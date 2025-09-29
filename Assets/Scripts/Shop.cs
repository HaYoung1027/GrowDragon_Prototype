using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
public class Shop : MonoBehaviour
{

    // ���� �̵� �ӵ� (�ν����Ϳ��� ���� ����)
    [SerializeField]
    private float moveSpeed = 2500f;

    // ���� ��ũ�� ������Ʈ
    [SerializeField]
    private GameObject ShopScreen;

    // RectTransform�� ĳ���� �θ� �� ������ GetComponent ȣ���� ���� �� ����.
    // ���� �𸣰ڴµ� transform.localPosition���� �̰� ���°� ����
    private RectTransform shopScreenRect;
    [SerializeField] private RectTransform[] buttons; // �̵��� ��ư 5��

    // ���� ���� ���� üũ
    private bool ShopOpen = false;

    // ���� ��ũ�� ����/�ݱ� ��ǥ Y��
    private float ScreenOpenY = -117f;
    private float ScreenCloseY = -670f;

    // ���� ��ũ�� ����/�ݱ� ��ǥ Y��
    private float ButtonOpenY = -365;
    private float ButtonCloseY = -918f;

    // ���� ��ư �̹���
    [SerializeField] private Sprite[] ShopButtonSprites;    // ��ư�� ��������Ʈ �迭 (����, ����)
    [SerializeField] private Image ShopButtonImage;         // ��ư�� Image ������Ʈ

    // ���� ���� ���� �ڷ�ƾ ���� (�ߺ� ���� ������)
    private Coroutine moveCoroutine;

    private void Start()
    {
        // UI ������Ʈ�� RectTransform�� ���� -> �̰� ������ ĳ��
        shopScreenRect = ShopScreen.GetComponent<RectTransform>();

        // �ʱ� ��ư �̹��� ���� (���� ����)
        ShopButtonImage.sprite = ShopButtonSprites[0];
    }

    // ���� ��ư�� Ŭ�� ��
    public void OnClickShopButton()
    {
        // ���� ���� ���� ����
        ShopOpen = !ShopOpen;

        // �ڷ�ƾ �ߺ� ����
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        // ��ư �̹��� ����
        ShopButtonImage.sprite = ShopOpen ? ShopButtonSprites[1] : ShopButtonSprites[0];

        // ��ǥ Y�� ����
        float ScreenTargetY = ShopOpen ? ScreenOpenY : ScreenCloseY;
        float ButtonTargetY = ShopOpen ? ButtonOpenY : ButtonCloseY;

        // �ڷ�ƾ ����, ����, (����x, ��ǥy) �� ����
        moveCoroutine = StartCoroutine(MoveShopScreen(ScreenTargetY, ButtonTargetY));
    }

    // ���� ��ũ���� ��ǥ ��ġ�� �̵���Ű�� �ڷ�ƾ (����x, ��ǥy) �� �޾ƿ�
    private IEnumerator MoveShopScreen(float screenTargetY, float buttonTargetY)
    {
        bool moving = true;

        while (moving)
        {
            moving = false;

            // 1) ShopScreen �̵�
            Vector2 screenCurrent = shopScreenRect.anchoredPosition;
            Vector2 screenNext = Vector2.MoveTowards(screenCurrent, new Vector2(screenCurrent.x, screenTargetY), moveSpeed * Time.deltaTime);
            shopScreenRect.anchoredPosition = screenNext;

            // ��ǥ ��ġ�� ���� �������� �ʾ����� ��� �̵�
            // Mathf.Abs(currentY - targetY): y�ุ ���
            // Distance(current, target) : ���� �Ÿ��� ��ȯ, x, y ��ü �Ÿ� ���
            if (Vector2.Distance(shopScreenRect.anchoredPosition, new Vector2(screenCurrent.x, screenTargetY)) > 0.01f)
                moving = true;

            // 2) ��ư�� �̵�
            for (int i = 0; i < buttons.Length; i++)
            {
                RectTransform buttonRect = buttons[i];
                Vector2 buttonCurrent = buttonRect.anchoredPosition;
                Vector2 buttonNext = Vector2.MoveTowards(buttonCurrent, new Vector2(buttonCurrent.x, buttonTargetY), moveSpeed * Time.deltaTime);
                buttonRect.anchoredPosition = buttonNext;

                // ��ǥ ��ġ�� ���� �������� �ʾ����� ��� �̵�
                if (Vector2.Distance(buttonRect.anchoredPosition, new Vector2(buttonCurrent.x, buttonTargetY)) > 0.01f)
                    moving = true;
            }

            yield return null;
        }

        // ������ ����
        shopScreenRect.anchoredPosition = new Vector2(shopScreenRect.anchoredPosition.x, screenTargetY);
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].anchoredPosition = new Vector2(buttons[i].anchoredPosition.x, buttonTargetY);

    }

}