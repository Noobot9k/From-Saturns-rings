using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveMarker : MonoBehaviour {

    public Transform Target;
    public Transform Player;
    public Camera Cam;
    public Image Icon;

    public Vector2 EdgePadding = new Vector2(48, 48);
    [Range(0, .5f)]
    public float IconPositionClamp = .5f;
    public float DisplayDistance = 20;

    void Start() {

    }
    void Update() {
        if(Target == null) { Icon.gameObject.SetActive(false); return; }
        Vector3 Pos = Cam.WorldToScreenPoint(Target.position, Camera.MonoOrStereoscopicEye.Mono);
        int height = Cam.pixelHeight;
        int width = Cam.pixelWidth;
        Pos = Vector3.ClampMagnitude(new Vector3(Pos.x / width, Pos.y / height, 0) - new Vector3(.5f, .5f, 0), IconPositionClamp);

        if (Player != null) Icon.gameObject.SetActive(Vector3.Distance(Target.position, Player.position) > Player.localScale.magnitude * DisplayDistance); //Pos.magnitude >= IconPositionClamp - .01f

        Pos = Vector3.Scale(Pos + new Vector3(.5f, .5f, 0), new Vector3(width, height, 0));
        Pos = new Vector3(Mathf.Clamp(Pos.x, EdgePadding.x, width - EdgePadding.x), Mathf.Clamp(Pos.y, EdgePadding.y, height - EdgePadding.y), Pos.z);
        Icon.rectTransform.position = Pos;
    }
}
