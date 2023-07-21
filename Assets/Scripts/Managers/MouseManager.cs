using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    Vector2 perPoint = Vector3.zero;
    Vector3 perMousePos = Vector3.zero;

    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;

    private Vector2 worldStartPoint;

    RaycastHit hit;

    

    private void Start()
    {
        StartCoroutine(CameraMove());
    }

    void LeftMouseDrag()
    {
        //print(current_position);
        // From the Unity3D docs: "The z position is in world units from the camera."  In my case I'm using the y-axis as height
        // with my camera facing back down the y-axis.  You can ignore this when the camera is orthograhic.
        current_position.z = hit_position.z = camera_position.y;

        // Get direction of movement.  (Note: Don't normalize, the magnitude of change is going to be Vector3.Distance(current_position-hit_position)
        // anyways.
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);

        // Invert direction to that terrain appears to move with the mouse.
        direction = direction * -1;

        
        Vector3 position = camera_position + direction;
        position.y = mainCamera.transform.position.y;
        position.z = mainCamera.transform.position.z;
        if(position.x > -9 && position.x < 10)
        {
            mainCamera.transform.position = position;
        }
        
    }

    private Vector2 getWorldPoint(Vector2 screenPoint)
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out hit);
        return hit.point;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            print("Down");
            hit_position = Input.mousePosition;
            camera_position = mainCamera.transform.position;

        }
#endif
    }

    IEnumerator CameraMove()
    {

       
        while (true)
        {


            
            if (!UICamera.Raycast(Input.mousePosition))
            {
#if UNITY_EDITOR
                if (Input.GetMouseButton(0))
                {
                    current_position = Input.mousePosition;
                    LeftMouseDrag();
                }
#endif
#if UNITY_ANDROID
                ZoomInOUt();
                // only work with one touch
                if (Input.touchCount == 1)
                {
                    Touch currentTouch = Input.GetTouch(0);

                    if (currentTouch.phase == TouchPhase.Began)
                    {
                        this.worldStartPoint = this.getWorldPoint(currentTouch.position);
                    }

                    if (currentTouch.phase == TouchPhase.Moved)
                    {
                        Vector2 worldDelta = this.getWorldPoint(currentTouch.position) - this.worldStartPoint;

                        print("worldDlta : " + worldDelta);
                        float x = mainCamera.transform.position.x - worldDelta.x;
                        float y = mainCamera.transform.position.y - worldDelta.y;
                        if (x > -9 && x < 10)
                        {
                            mainCamera.transform.Translate(
                           -worldDelta.x,
                           0,
                           0
                            );
                        }

                            if (y < (22.8f + 10) && y > (22.8f - 10))
                            {
                                mainCamera.transform.Translate(
                               0,
                               -worldDelta.y,
                               0
                           );
                           }
                       
                    }
                }
#endif
            }

            


            yield return new WaitForEndOfFrame();
        }
    }

    public float perspectiveZoomSpeed = 0.5f;  //줌인,줌아웃할때 속도(perspective모드 용)      
    public float orthoZoomSpeed = 0.5f;      //줌인,줌아웃할때 속도(OrthoGraphic모드 용)  
    void ZoomInOUt()
    {




        if (Input.touchCount == 2) //손가락 2개가 눌렸을 때
        {
            Touch touchZero = Input.GetTouch(0); //첫번째 손가락 터치를 저장
            Touch touchOne = Input.GetTouch(1); //두번째 손가락 터치를 저장

            //터치에 대한 이전 위치값을 각각 저장함
            //처음 터치한 위치(touchZero.position)에서 이전 프레임에서의 터치 위치와 이번 프로임에서 터치 위치의 차이를 뺌
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition는 이동방향 추적할 때 사용
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // 각 프레임에서 터치 사이의 벡터 거리 구함
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude는 두 점간의 거리 비교(벡터)
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // 거리 차이 구함(거리가 이전보다 크면(마이너스가 나오면)손가락을 벌린 상태_줌인 상태)
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // 만약 카메라가 OrthoGraphic모드 라면
            if (mainCamera.orthographic)
            {
                mainCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, 0.1f);
            }
            else
            {
                mainCamera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
                mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 20f,60f);
            }
        }

    }
}
