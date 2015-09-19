using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Net;

[AddComponentMenu("UI/Image Downloader")]
public class CUIImageDownloader : CUIImage {

	#region Properties
	private Image 	m_ImageSprite;
	private string 	m_StorePath;
	private byte 	_ImageBytes;
	private bool 	_IsRun = false;
	private RectTransform m_RectTransform;
	private float m_ContentWidth;
	private float m_ContentHeight;

	[SerializeField]
	private Camera m_CameraControl;
	public Camera CameraControl {
		get { return m_CameraControl; }
		set { m_CameraControl = value; }
	}
	[SerializeField]
	private string m_ImageName = "Noimage";
	public string ImageName {
		get { return m_ImageName; }
		set { m_ImageName = value; }
	}
	[SerializeField]
	private string m_ImageType;
	public string ImageType {
		get { return m_ImageType; }
		set { m_ImageType = value; }
	}
	[SerializeField]
	private string m_ImagePath = "https://s-media-cache-ak0.pinimg.com/736x/88/f6/b9/88f6b97c37b9101283d9a22259a4de3f.jpg";
	public string ImagePath {
		get { return m_ImagePath; }
		set { m_ImagePath = value; }
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


	#endregion

	#region MonoBehaviour

	public void Init () {
		m_ImageSprite 	= this.GetComponent<Image>();
		m_RectTransform = this.GetComponent<RectTransform>();
		m_StorePath		= string.Format("{0}/{1}_{2}x{3}", Application.temporaryCachePath, m_ImageName, m_ImageWidth, m_ImageHeight);
		m_ContentWidth  = this.m_CameraControl.pixelWidth;
		m_ContentHeight = this.m_CameraControl.pixelHeight;
		this.FitContent();
	}

	#endregion

	#region Main Method

	public void StartDownload() {
		if (_IsRun == false) {
			_IsRun = true;
			StartCoroutine (DownloadWWW());
		}
	}

	public void StopDownload() {
		if (_IsRun == true) {
			_IsRun = false;
			StopCoroutine (DownloadWWW());
		}
	}

	private IEnumerator DownloadWWW() {
		WWW _www = null ;
		var _needSave = false;
		if (File.Exists (m_StorePath)) {
			var localPath = string.Format ("file:///{0}", m_StorePath);
			_www = new WWW (localPath);
		} else {
			var imageURL = string.Format ("{0}{1}.{2}", m_ImagePath, m_ImageName, m_ImageType);
			_www = new WWW (imageURL);
			_needSave = true;
		}
		var waiter = new WaitForSeconds (0.001f);
		while (!_www.isDone) {
			m_ImageSprite.color = new Color(m_ImageSprite.color.r, m_ImageSprite.color.g, m_ImageSprite.color.b, _www.progress);
			yield return waiter;
		}
		yield return _www;
		m_ImageSprite.color = new Color(m_ImageSprite.color.r, m_ImageSprite.color.g, m_ImageSprite.color.b, 1f);
		var bytes = _www.bytes;
		if (_needSave) {
			SaveImage (m_StorePath, bytes);
		}
		LoadImage (bytes);
	}	

	private void SaveImage(string path, byte[] bytes) {
		var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
		fs.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback(EndWriteCallback), fs);
	} 

	private void LoadImage(byte[] bytes) {
		var textureWWW = new Texture2D (m_ImageWidth, m_ImageHeight, TextureFormat.ARGB32, false);
		textureWWW.LoadImage (bytes);
		var spriteWWW = Sprite.Create (textureWWW, new Rect (0, 0, textureWWW.width, textureWWW.height), Vector2.zero);
		m_ImageSprite.sprite = spriteWWW;
	}

	private void LoadImage(string path) {
		var bytes = File.ReadAllBytes (path);
		LoadImage (bytes);
	}

	private void LoadImageFromResource(string name) {
		var noImageSprite = Resources.Load <Sprite> (string.Format ("Images/", name));
		m_ImageSprite.sprite = noImageSprite;
	}

	private void EndWriteCallback(IAsyncResult result) {
		FileStream fs = result.AsyncState as FileStream;
		fs.EndWrite(result);
		fs.Flush();
		fs.Close();
	}



	#endregion

}
