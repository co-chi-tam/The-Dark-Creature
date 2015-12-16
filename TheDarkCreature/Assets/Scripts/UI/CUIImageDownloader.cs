using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

[AddComponentMenu("UI/Image Downloader")]
public class CUIImageDownloader : CUIImage, IUserInterface {

	#region Properties
	[SerializeField]
	private string 	m_StorePath;
	public string StorePath {
		get { return m_StorePath; }
	}

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
		m_StorePath		= Application.persistentDataPath + "/" + m_DirectoryPath + "/" + m_ImageName + ".png";

		this.color 		= Color.white;
		m_AbsoluteImageURL = m_ImagePath + m_ImageName + "." + m_ImageType;

		var dirTemp =  Application.persistentDataPath + "/" + m_DirectoryPath;
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
		} else {
			LoadImageFromLocalFile (url);
		}
	}

	public void StartDownload(Action complete = null, Action<string> error = null) {
		if (string.IsNullOrEmpty (m_ImagePath)) {
			LoadResources (m_ImageName);
			if (complete != null) {
				complete();
			}
			return;
		}
		if (m_ImagePath.StartsWith ("http://") || m_ImagePath.StartsWith ("https://")) {
			DownLoadCNetRequest(complete, error);
		} else {
			var realPath = m_ImagePath + (m_ImageName);
			LoadResources (realPath);
			if (complete != null) {
				complete();
			}
		}
	}

	public void StopDownload() {
//		StopCoroutine (DownloadWWW());
	}

	public void LoadResources(string path) {
		var textureWWW = Resources.Load<Texture2D> (path);
		if (textureWWW != null) {
			var spriteWWW = Sprite.Create (textureWWW, new Rect (0, 0, textureWWW.width, textureWWW.height), Vector2.zero, m_PixelPerUnit);
			this.sprite = spriteWWW;
		} else {
			Debug.LogError ("Image path "+ (path));
		}
	}

	private void DownLoadCNetRequest(Action complete = null, Action<string> error = null) {
		if (string.IsNullOrEmpty (m_ImagePath)) return;
		/*if (m_Request == null) {
			m_Request = CNetRequest.GetInstance ();
		}
		var realPath = string.Empty;
		if (File.Exists (m_StorePath)) {
			realPath = "file:///".Append (m_StorePath);
			LoadImage (m_StorePath);
			if (complete != null) {
				complete();
			}
		} else {
			realPath = m_ImagePath.Append (m_ImageName, ".", m_ImageType);
			m_Request.Request (realPath, (x) => {
				var bytes = x.Bytes;
				if (m_NeedSave) {
					StartCoroutine (SaveImage (m_StorePath, bytes, () => {
						if (complete != null) {
							complete();
						}
						LoadImage (m_StorePath);
					}));
				} else {
					if (complete != null) {
						complete();
					}
					LoadImage (bytes);
				}
			}, (y) => {
				Debug.LogError (y.Append (" / ", gameObject.name, " / ", realPath));
				if (error != null) {
					error (y.Append (" / ", gameObject.name, " / ", realPath));
				}
			});
		}*/
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
		m_Texture2D = null;
	}

	private void LoadImage(Texture2D textureWWW) {
		this.sprite = Sprite.Create (textureWWW, new Rect (0, 0, textureWWW.width, textureWWW.height), Vector2.zero, m_PixelPerUnit);
		textureWWW = null;
	}

	public void LoadImage(string path) {
		var bytes = File.ReadAllBytes (path);
		LoadImage (bytes);
	}

	private void LoadImageFromResource(string name) {
		var noImageSprite = Resources.Load <Sprite> ("Images/" + (name));
		this.sprite = noImageSprite;
	}
	
	private void LoadImageFromLocalFile(string path) {
		LoadImage (path);
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

	public void ChangeSprite(Sprite newSprite) {
		StartCoroutine (DoChangeSprite(newSprite));
	}

	private IEnumerator DoChangeSprite(Sprite newSprite, float speed = 0.05f) {
		var currentColor = this.color;
		var waiter = new WaitForFixedUpdate();
		this.sprite = newSprite != null ? newSprite : this.sprite;
//		this.color = new Color (this.color.r, this.color.g, this.color.b, 0.25f);
//		while (this.color.a < 1f) {
//			this.color = new Color (this.color.r, this.color.g, this.color.b, 
//			                        Mathf.Clamp (this.color.a + speed, 0f, 1f));
			yield return waiter;
//		}
		this.color = currentColor;
	}

	#endregion

}
