using UnityEngine;
using UnityEngine.UI;
using Effect;

public class UISlot : MonoBehaviour {

	#region Properties
	private UIInventory.SelectedSlot m_SelectedSlot;
	public UIInventory.SelectedSlot OnSelectedSlot {
		get { return m_SelectedSlot; }
		set { m_SelectedSlot = value; }
	}

	[SerializeField]
	private int m_SlotIndex = 0;
	public int SlotIndex
	{
		get { return m_SlotIndex; }
		set { m_SlotIndex = value; }
	}
	[SerializeField]
	private Image m_IconImage;
	[SerializeField]
	private Text m_AmountItem;
	[SerializeField]
	private Button m_ItemButton;
	[SerializeField]
	private CUIEventDragManager m_DragManager;

	private UIItemController m_ItemController;
	private Sprite m_NoImage;
	private Vector3 m_StartPosition;
	private bool m_StartDrag = false;

	#endregion 

	#region Mono Behaviour

	void Start() {
		m_ItemButton.onClick.AddListener (() => {
			OnPointerClick();
		});
		m_NoImage = TDCUltilities.LoadImage ("NoImage");

		m_DragManager = this.GetComponent<CUIEventDragManager>();
		m_DragManager.EventOnBeginDrag += OnBeginDrag;
		m_DragManager.EventOnDrag += OnDrag;
		m_DragManager.EventOnEndDrag += OnEndDrag;

		m_StartPosition = this.transform.localPosition;
	}

	#endregion 

	#region Main methods

	private void OnBeginDrag(GameObject obj, Vector3 pos) {
		
		m_StartDrag = true;
	}

	private void OnDrag(GameObject obj, Vector3 pos) {
		pos.z = 10f;
		obj.transform.position = Camera.main.ScreenToWorldPoint (pos);
	}

	private void OnEndDrag(GameObject obj, Vector3 pos) {
		obj.transform.localPosition = m_StartPosition;
		m_StartDrag = false;
	}

	public void LoadSlot(UIItemController item) {
		m_ItemController = item;
		m_IconImage.sprite = TDCUltilities.LoadImage (m_ItemController.GetData().Icon);
		m_ItemController.GetData().GetChangeValue<int> ("Amount", ChangeAmount);
		m_AmountItem.text = m_ItemController.GetData().Amount.ToString ();
	}

    public void LoadSlot(int index, UIItemController item) {
		m_SlotIndex = index;
		LoadSlot(item);
	}

	private void ChangeAmount(int source, int target) {
		m_AmountItem.text = target.ToString ();
	}

	public void EmptySlot() {
		m_ItemController = null;
		m_IconImage.sprite = m_NoImage;
		m_AmountItem.text = string.Empty;
		m_SelectedSlot = null;
	}

	private void OnPointerClick() {
		if (m_SelectedSlot != null) {
			m_SelectedSlot (m_SlotIndex);
		}
	}

	#endregion

}
