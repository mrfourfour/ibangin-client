using System;
using System.Collections;
using UnityEngine;

using Wolf3D.ReadyPlayerMe.AvatarSDK;

using static Wolf3D.ReadyPlayerMe.AvatarSDK.ExtensionMethods;

public class TestUseHandler : MonoBehaviour
{
    private const string MouthOpenBlendShapeName = "mouthOpen";

    private SkinnedMeshRenderer headMesh;
    private SkinnedMeshRenderer beardMesh;
    private SkinnedMeshRenderer teethMesh;

    private int mouthOpenBlendShapeIndexOnHeadMesh = -1;
    private int mouthOpenBlendShapeIndexOnBeardMesh = -1;
    private int mouthOpenBlendShapeIndexOnTeethMesh = -1;

    Transform upperLip;
    Transform lowerLip;
    private float defaultLipDistance = 0;
    static class SigmoidConstant
    {
        public const float Default = 0.5f;
    }

    //public GameObject testFace;

    // Start is called before the first frame update
    void Start()
    {
        GetMeshAndSetIndex(MeshType.HeadMesh, ref headMesh, ref mouthOpenBlendShapeIndexOnHeadMesh);
        GetMeshAndSetIndex(MeshType.BeardMesh, ref beardMesh, ref mouthOpenBlendShapeIndexOnBeardMesh);
        GetMeshAndSetIndex(MeshType.TeethMesh, ref teethMesh, ref mouthOpenBlendShapeIndexOnTeethMesh);

        // 리스트로 블렌드 쉐이프 정보 받기
        // testFace.GetComponentsInChildren<SkinnedMeshRenderer>();

        upperLip = transform.Find("Upper Lip").gameObject.transform;
        lowerLip = transform.Find("Lower Lip").gameObject.transform;

        defaultLipDistance = (upperLip.position - lowerLip.position).sqrMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        float curLipDistance = (upperLip.position - lowerLip.position).sqrMagnitude;
        var mouseOpenValue = sigmoid(Convert.ToDouble((curLipDistance - defaultLipDistance) / defaultLipDistance));
        SetBlendshapeWeights(mouseOpenValue);
    }

    private float sigmoid(double value)
    {
        float sigmoidValue = Convert.ToSingle(1.0f / (1.0f + Math.Exp(-value)));
        if (sigmoidValue <= SigmoidConstant.Default)
            return 0.0f;

        return sigmoidValue;
    }

    private void GetMeshAndSetIndex(MeshType meshType, ref SkinnedMeshRenderer mesh, ref int index)
    {
        mesh = gameObject.GetMeshRenderer(meshType);

        if (mesh != null)
        {
            index = mesh.sharedMesh.GetBlendShapeIndex(MouthOpenBlendShapeName);
        }
    }

    private void SetBlendshapeWeights(float weight)
    {
        SetBlendShapeWeight(headMesh, mouthOpenBlendShapeIndexOnHeadMesh);
        SetBlendShapeWeight(beardMesh, mouthOpenBlendShapeIndexOnBeardMesh);
        SetBlendShapeWeight(teethMesh, mouthOpenBlendShapeIndexOnTeethMesh);

        void SetBlendShapeWeight(SkinnedMeshRenderer mesh, int index)
        {
            if (index >= 0)
            {
                mesh.SetBlendShapeWeight(index, weight * 100f);
            }
        }
    }
}