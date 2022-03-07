using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public Texture2D cursorIdle;
    public Texture2D cursorGrab;
    CursorMode cursorMode = CursorMode.Auto;

    TrailRenderer[] skidRenderer;
    ParticleSystem[] smokePartArr;

    Ray ray;
    //Plane surface;
    Plane movePlane;

    Component[] smokeVfx;
    bool isBraking = false;

    public WheelCollider wheelColFR;
    public WheelCollider wheelColFL;
    public WheelCollider wheelColBR;
    public WheelCollider wheelColBL;

    public float acceleration = 50000f;

    public float brake = 40000f;

    WheelCollider[] colArr;

    Vector3 point;

    float hitDist;
    private float currentTurnAngle = 0f;

    void Start()
    {

        smokePartArr = GetComponentsInChildren<ParticleSystem>();
        colArr = GetComponentsInChildren<WheelCollider>();

        smokeVfx = GetComponentsInChildren<ParticleSystem>();
        Cursor.visible = true;

        skidRenderer = GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer skid in skidRenderer)
        {
            skid.emitting = false;
        }
        
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorGrab, Vector2.zero, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(cursorIdle, Vector2.zero, cursorMode);
    }

    void reverse(WheelCollider[] wC, float decel)
    {

        foreach(WheelCollider wCol in wC)
        {
            wCol.motorTorque = decel;
        }

    }

    void stopSmoke(ParticleSystem[] sPA)
    {
        foreach(ParticleSystem pS in sPA)
            {
                pS.Stop(true);
            }
    }

    void UpdateWheel(WheelCollider col, Transform trans){

        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
   }

    void OnMouseDrag()
    {

            wheelColFL.motorTorque = acceleration * (Input.GetMouseButton(0) ? 1 : Input.GetMouseButton(1) ? -1 : 0) * Time.fixedDeltaTime;
            wheelColFR.motorTorque = acceleration * (Input.GetMouseButton(0) ? 1 : Input.GetMouseButton(1) ? -1 : 0) * Time.fixedDeltaTime;
            //wheelColBL.motorTorque = acceleration * (Input.GetMouseButton(0) ? 1 : Input.GetMouseButton(1) ? -1 : 0) * Time.fixedDeltaTime;
            //wheelColBR.motorTorque = acceleration * (Input.GetMouseButton(0) ? 1 : Input.GetMouseButton(1) ? -1 : 0) * Time.fixedDeltaTime;

            movePlane = new Plane(-Camera.main.transform.forward,transform.position); // find a parallel plane to the camera based on obj start pos;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);// shoot a ray at the obj from mouse screen point

            if (movePlane.Raycast(ray,out hitDist))
            {

                point = ray.GetPoint(hitDist);
                
                Vector3 targetDir = point - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                Vector3 relativPos = transform.InverseTransformPoint(point);
                if (relativPos.x >0)
                {
                    currentTurnAngle = (angle* 1000) * (Input.GetMouseButton(0) ? 1 : Input.GetMouseButton(1) ? -1 : 0) * Time.fixedDeltaTime;
                }else{
                    currentTurnAngle = (angle* 1000) * (Input.GetMouseButton(0) ? -1 : Input.GetMouseButton(1) ? 1 : 0) * Time.fixedDeltaTime;
                }
                
                wheelColFR.steerAngle = currentTurnAngle * Time.fixedDeltaTime;
                wheelColFL.steerAngle = currentTurnAngle * Time.fixedDeltaTime;

            }

        if(isBraking)
        {
            foreach(ParticleSystem smoke in smokeVfx)
            {
                smoke.Play();
            }

            foreach(TrailRenderer skid in skidRenderer)
            {
                skid.emitting = true;
            }    

        }else if(!isBraking){
            foreach(ParticleSystem smoke in smokeVfx)
            {
                smoke.Stop();
            }

            foreach(TrailRenderer skid in skidRenderer)
            {
                skid.emitting = false;
            }

        }

    }

    void Update()
    {

        if(Input.GetMouseButton(1))
        {
            
            isBraking = true;
            wheelColFL.brakeTorque = brake;
            wheelColFR.brakeTorque = brake;
            wheelColBL.brakeTorque = brake;
            wheelColBR.brakeTorque = brake;

        }else if(Input.GetMouseButtonUp(1)){

            isBraking = false;
            wheelColFL.brakeTorque = 0f;
            wheelColFR.brakeTorque = 0f;
            wheelColBL.brakeTorque = 0f;
            wheelColBR.brakeTorque = 0f;
            stopSmoke(smokePartArr);
            

        }else if(Input.GetKey(KeyCode.Space)){

            reverse(colArr, -1000);

        }else if(Input.GetKeyUp(KeyCode.Space)){

            reverse(colArr, 0);
        }
        
        if(Input.GetMouseButtonUp(0)) {
            wheelColFL.motorTorque = 0f;
            wheelColFR.motorTorque = 0f;
        }

    }

    
}