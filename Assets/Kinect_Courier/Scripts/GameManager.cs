using com.rfilkov.components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerRoleModel
{
    BlackGirl,
    Aottman,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public delegate void S2CFuncAction<T>(T info);
    public Dictionary<string, S2CFuncAction<string>> S2CFuncTable = new Dictionary<string, S2CFuncAction<string>>();


    public ZPoseHelper PoseHelper;
    public static bool Join = false;



    // 模型角色缓存
    public Dictionary<PlayerRoleModel, GameObject> RoleTables = new Dictionary<PlayerRoleModel, GameObject>();
    // 当前游戏角色
    public PlayerRoleModel CurPlayerRoleModel = PlayerRoleModel.BlackGirl;
    // 当前游戏角色实例
    public GameObject CurRole = null;

    public GameObject AottmanRole = null;

    public Transform RoleDatabase;

    void Start()
    {
        Instance = this;
        PoseHelper.Init(CurPlayerRoleModel);
        S2CFuncTable.Add(S2CFuncName.ChangeRole, S2C_UpdatePlayerRole);
        RoleTables.Add(CurPlayerRoleModel, CurRole.gameObject);
    }

    private void Update()
    {
        if (!runUpdate && Join)
        {
            StartCoroutine("StartUpdataPose");
        }
    }

    bool runUpdate = false;
    public IEnumerator StartUpdataPose()
    {
        runUpdate = true;
        while (true)
        {
            yield return new WaitForSeconds(1f / 45);
            UpdatePose();
        }
    }

    private void UpdatePose()
    {
        PoseHelper.FreshPoseData();
        MessageManager.Instance.SendUpdatePose(JsonUtility.ToJson(PoseHelper.PoseData));
    }

    private void S2C_UpdatePlayerRole(string param)
    {
        StopCoroutine("StartUpdataPose");

        Debug.Log("-------S2C_UpdatePlayerRole");

        PlayerRoleModel role = (PlayerRoleModel)int.Parse(param);

        //if (CurRole != null)
        //{
        //    GameObject.Destroy(CurRole.gameObject);
        //    //CurRole.SetActive(false);
        //    CurRole = null;
        //}

        CurRole = null;

        GameObject model;
        if (RoleTables.TryGetValue(role, out model))
        {
            CurRole = model;
            CurRole.SetActive(true);
        }
        else
        {
            CurRole = AottmanRole;//GameObject.Instantiate(Resources.Load<GameObject>("Model/" + role.ToString()));
            CurRole.SetActive(true);
            RoleTables[role] = CurRole.gameObject;
        }


        if (role != CurPlayerRoleModel)
        {
            RoleTables[CurPlayerRoleModel].transform.SetParent(RoleDatabase);
            RoleTables[CurPlayerRoleModel].SetActive(false);
            CurPlayerRoleModel = role;
            CurRole.transform.SetParent(PoseHelper.transform);
            CurRole.transform.localPosition = Vector3.zero;
            CurRole.transform.localRotation = Quaternion.identity;
            PoseHelper.Init(role);
        }

        StartCoroutine("StartUpdataPose");

        Debug.Log(role);
        MessageManager.Instance.SendChangeRole((int)role);



    }
}
