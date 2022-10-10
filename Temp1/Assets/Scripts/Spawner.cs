using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{


    //생성할 몬스터 프리팹, 몬스터가 생성되는 위치 계산을 위한 벽 오브젝트
    public GameObject enemyObject, Wall;

    //생성된 몬스터 수를 저장할 매니저 스크립트
    //시작할 때 메인 매니저에게 값을 넘긴다. 메인 매니저는 넘겨진 값들을 합해서 총 몬스터 수를 구한다.
    public MainManager mainManager;

    //변수 floor에는 Ground 오브젝트 넣으면 됩니다.
    public BoxCollider floor;

    //생성할 몬스터 수
    public int numOfEnemy = 5; 

    //floor오브젝트의 위치값을 저장 및 랜덤함수를 돌릴 변수
    Vector3 sizeOfGround;

    //몬스터 생성 위치가 올바를 때만 false가 되는 bool 변수
    bool resetPosition = true;

    //몬스터가 죽을 때마다 +1이 될 변수. 이 변수가 numOfEnemy와 같아지면 포탈 활성화. 
    //추가로 스테이지 클리어 UI가 있었으면 좋겠습니다. to 해인님 from 동욱
    int count = 0;
    private void Start()
    {
        //메인 매니저에게 생성될 몬스터의 수를 넘긴다.
        if (mainManager == null)
        {
            mainManager = FindObjectOfType<MainManager>();
        }
        mainManager.numOfStageEnemy += numOfEnemy;

        //차일드 0번부터 순서대로 오른쪽 왼쪽 위 아래 벽. 순서가 맞아야 아래 이프문이 정상 실행
        Vector3[] WallPosition; 
        WallPosition = new Vector3[Wall.transform.childCount];

        //Wall 오브젝트의 차일드 오브젝트 좌우위아래 대입
        for(int i = 0; i < Wall.transform.childCount; i++)
        {
            WallPosition[i] = Wall.transform.GetChild(i).position;
        }

        //몬스터 생성. 
        for(int i= 0; i < numOfEnemy; i++)
        {
            //변수 초기화. 꼭 필요함
            resetPosition = true;

            //몬스터가 벽 안에 생성되는지 if로 계산하고, 벽 안이 아니면 계속 돌아가는 while문 
            //(벽 안에 생성되면 resetPosition = false) 
            while (resetPosition)
            {
                //바닥 오브젝트 floor의 콜리더 사이즈를 구해서(Vector3값으로 받음) Random.value를 곱하는 식.
                //이 식으로 바닥 콜리더 크기에 맞게 위치값을 계산.. 그런데 콜리더 크기가 더 커서 그런지 자꾸 벽을 넘어감
                //그래서 바로 아래에 if문으로 벽 안에 있는지 판독하게 만들었음.
                //아마 콜리더 크기가 더 큰게 콜리더 /2를 하면 해결 될지도 모르겠음 일단 if문으로 해결하고 나중에 확인할 예정
                sizeOfGround = new Vector3(floor.size.x * Random.value, floor.size.y, floor.size.z * Random.value);
                if (WallPosition[0].x < sizeOfGround.x ||   //오른쪽 벽
                    WallPosition[1].x > sizeOfGround.x ||   //왼쪽 벽
                    WallPosition[2].z < sizeOfGround.z ||   //윗 벽
                    WallPosition[3].z > sizeOfGround.z)     //아래 벽
                {
                    //얼마나 while문이 반복되었는지 알 수 있음. 추후 오차값을 줄이는 데 식별용으로 쓰일 예정
                    Debug.Log("Enemy 오브젝트 위치 이탈. 재조정"); 
                }
                else
                {
                    //몬스터 오브젝트 생성.
                    Instantiate(enemyObject, sizeOfGround, Quaternion.identity);
                    //while문 종료. for문은 계속 실행되기 때문에 초기에 설정한 numOfEnemy 수에 맞게 몬스터 생성이 됨
                    resetPosition = false;
                }
            }
        }
    }

    //나중에 게임 매니저로 옮겨야 되는 함수. 
    void ClearStage()
    {
        //portal.transform.position = 
        //portal.gameObject.SetActive(true);
    }

}
