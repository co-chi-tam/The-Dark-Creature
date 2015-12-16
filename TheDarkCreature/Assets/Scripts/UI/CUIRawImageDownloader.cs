using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class CUIRawImageDownloader : CUIRawImage, IUserInterface {

	#region Properties
	[SerializeField]
	private string 	m_StorePath;
	private byte 	_ImageBytes;
	[SerializeField]
	private string	m_AbsoluteImageURL;
	public string AbsoluteImageURL {
		get { return m_AbsoluteImageURL; }
	}
	
	[SerializeField]
	private string m_ImageName = "";
	public string ImageName {
		get { return m_ImageName; }
		set { m_ImageName = value; }
	}
	[SerializeField]
	private string m_ImageType = "";
	public string ImageType {
		get { return m_ImageType; }
		set { m_ImageType = value; }
	}
	[SerializeField]
	private string m_ImagePath = "";
	public string ImagePath {
		get { return m_ImagePath; }
		set { m_ImagePath = value; }
	}
	[SerializeField]
	private string m_DirectoryPath = "images";
	public string DirectoryPath {
		get { return m_DirectoryPath; }
		set { m_DirectoryPath = value; }
	}
	[SerializeField]
	private int m_ImageWidth = 1024;
	public int ImageWidth {
		get { return m_ImageWidth; }
		set { m_ImageWidth = value; }
	}
	[SerializeField]
	private int m_ImageHeight = 1024;
	public int ImageHeight {
		get { return m_ImageHeight; }
		set { m_ImageHeight = value; }
	}
	[SerializeField]
	private int m_PixelPerUnit = 1024;
	public int PixelPerUnit {
		get { return m_PixelPerUnit; }
		set { m_PixelPerUnit = value; }
	}
	
	[SerializeField]
	private bool m_NeedSave = true;
	public bool NeedSave {
		get { return m_NeedSave; }
		set { m_NeedSave = value; }
	}

	private Texture2D m_Texture2D;
	
	#endregion
	
	#region MonoBehaviour
	
	public override void Init (string directory = "images") {
		base.Init();
		m_DirectoryPath = directory;
		m_StorePath		= string.Format("{0}/{1}/{2}.png", Application.persistentDataPath, m_DirectoryPath, m_ImageName);
		this.color 		= Color.white;
		m_AbsoluteImageURL = string.Format ("{0}{1}.{2}", m_ImagePath, m_ImageName, m_ImageType);
		
		var dirTemp = string.Format("{0}/{1}", Application.persistentDataPath, m_DirectoryPath);
		if (!System.IO.Directory.Exists (dirTemp)) {
			System.IO.Directory.CreateDirectory (dirTemp);
		}
		if (m_Texture2D == null) {
			m_Texture2D = new Texture2D (m_ImageWidth, m_ImageHeight, TextureFormat.PVRTC_RGBA2, false);
		}
	}
	
	protected override void Start ()
	{
		base.Start ();
		m_Texture2D = new Texture2D (m_ImageWidth, m_ImageHeight, TextureFormat.PVRTC_RGBA2, false);
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		m_Texture2D = null;
	}
	
	#endregion
	
	#region Main Method
	
	public void SetupImageDownload(string url, string directory = "images") {
		if (url.StartsWith ("http://") || url.StartsWith ("https://")) {
			try {
				var uri = new Uri (url);
				var fileNameWithType = Path.GetFileName (uri.LocalPath);
				
				var filePath = uri.OriginalString.Substring(0, uri.OriginalString.IndexOf (fileNameWithType));
				var fileName = fileNameWithType.Substring (0, fileNameWithType.IndexOf ('.'));
				var fileType = fileNameWithType.Substring (fileNameWithType.IndexOf ('.') + 1, fileNameWithType.Length - fileNameWithType.IndexOf ('.') - 1);
				
				m_ImagePath = filePath;
				m_ImageName = fileName;
				m_ImageType = fileType;
				
				Init (directory);
			} catch (Exception ex) {
				Debug.LogError (string.Format ("[CUIImageDownloader] Error {0} - {1}", ex.Message, url));
			}
		}
	}
	
	public void StartDownload(Action complete = null) {
		if (string.IsNullOrEmpty (m_ImagePath)) {
			LoadResources (m_ImageName);
			return;
		}
		if (m_ImagePath.StartsWith ("http://") || m_ImagePath.StartsWith ("https://")) {
			DownLoadCNetRequest(complete);
		} else {
			var realPath = string.Format ("{0}/{1}", m_ImagePath, m_ImageName);
			LoadResources (realPath);
		}
	}
	
	public void StopDownload() {
//		StopCoroutine (DownloadWWW());
	}
	
	public void LoadResources(string path) {
		var textureWWW = Resources.Load<Texture2D> (string.Format("{0}", path));
		if (textureWWW != null) {
			this.texture = textureWWW;
		} else {
			Debug.LogError ("Image path " + path);
		}
	}
	
	private void DownLoadCNetRequest(Action complete = null) {
		if (string.IsNullOrEmpty (m_ImagePath)) return;
//		var realPath = "";
		if (File.Exists (m_StorePath)) {
//			realPath = string.Format ("file:///{0}", m_StorePath);
			LoadImage (m_StorePath);
			if (complete != null) {
				complete();
			}
		} else {
//			realPath = string.Format ("{0}{1}.{2}", m_ImagePath, m_ImageName, m_ImageType);
		}
	}
	
	private IEnumerator SaveImage(string path, byte[] bytes, Action saveComplete = null) {
		var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, false);
		fs.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback (EndWriteCallback), fs);
		yield return File.Exists (path);
		if (saveComplete != null) {
			saveComplete();
		}
	} 
	
	private void LoadImage(byte[] bytes) {
		if (m_Texture2D == null) {
			m_Texture2D = new Texture2D (m_ImageWidth, m_ImageHeight, TextureFormat.PVRTC_RGBA2, false);
		}
		m_Texture2D.LoadImage (bytes);
		LoadImage (m_Texture2D);
	}
	
	private void LoadImage(Texture2D textureWWW) {
		this.texture = textureWWW;
	}
	
	public void LoadImage(string path) {
		var bytes = File.ReadAllBytes (path);
		LoadImage (bytes);
	}
	
	private void LoadImageFromResource(string name) {
		var noImageSprite = Resources.Load <Texture2D> (string.Format ("Images/", name));
		this.texture = noImageSprite;
	}
	
	private void EndWriteCallback(IAsyncResult result) {
		var fs = result.AsyncState as FileStream;
		fs.EndWrite(result);
		fs.Flush();
		fs.Close();
	}
	
	public override void FitContent() {
		base.FitContent();
	}
	
	public override void CalculateContent() {
		base.CalculateContent();
	}
	
	public override void MoveToFirst() {
		base.MoveToFirst();
	}
	
	public void ChangeTexture(Texture newTexture) {
		this.texture = newTexture != null ? newTexture : this.texture;
	}

	public void ChangeSprite(Sprite newSprite) {
		ChangeTexture (newSprite.texture);
	}
	
	#endregion
	
}
