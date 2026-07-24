using UnityEngine;

public class RayTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 왼쪽 버튼을 클릭했을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 메인 카메라에서 마우스 위치로 Ray 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            // Ray가 어떤 오브젝트와 충돌했는지 검사
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("맞은 오브젝트 : " + hit.collider.gameObject.name);
            }
        }
    }
}
