using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct ZPose
{
    public void Set(int c, PlayerRoleModel pm)
    {
        count = c;
        position = new Vector3[c];
        rotation = new Quaternion[c];
        role = pm;
    }
    public PlayerRoleModel role;
    public int count;
    public Vector3[] position;
    public Quaternion[] rotation;
}
public class ZPoseHelper : MonoBehaviour
{
    private bool m_Initialized = false;
    protected Transform RootNode;
    public List<GameObject> ModelNodes;


    public ZPose PoseData;

    #region Unity_Internal


    #endregion

    public void Init(PlayerRoleModel pm)
    {

        RootNode = transform;

        ModelNodes.Clear();

        InitAllNodes(RootNode);

        Debug.Log(ModelNodes.Count);

        PoseData = new ZPose();
        PoseData.Set(ModelNodes.Count, pm);
    }

    private void InitAllNodes(Transform trans)
    {
        foreach (Transform item in trans)
        {
            if (item != null && item.gameObject.activeInHierarchy)
            {
                ModelNodes.Add(item.gameObject);
                InitAllNodes(item);
            }
        }
    }

    public void FreshPoseData()
    {
        for (int i = 0; i < PoseData.count; i++)
        {
            //if (i >= ModelNodes.Count || ModelNodes[i] == null)
            //{
            //    ModelNodes.Clear(); // 耦合的地方很多
            //    InitAllNodes(RootNode);
            //    return;
            //}
            PoseData.position[i] = ModelNodes[i].transform.localPosition;
            PoseData.rotation[i] = ModelNodes[i].transform.localRotation;
        }
    }
}
