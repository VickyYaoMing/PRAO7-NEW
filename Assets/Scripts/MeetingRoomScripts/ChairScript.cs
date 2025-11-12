using System.Collections;
using UnityEngine;
/// <summary>
/// have two vectors that checks rotation and one that checks postion on x,y values
/// basesclass has bool iscleaned and method on mouse click
/// in apply state check if items are set to celaned or messy
/// if not cleaned set the messy pos, if cleaned set the cleaned pos
/// inClick check isCleaned state on item and if false set to cleaned
/// if cleaned do nothing
/// 
/// </summary>
public class chair_scripts : ClickableObjects
{
    [SerializeField] private float cleanXPos;
    [SerializeField] private float messyXPos;

    [SerializeField] protected float cleanYRot;
    [SerializeField] protected float messyRot;
    private float animationTime = 1f;


    Vector3 pos;
    Vector3 rot;
    private void Start()
    {
        ApplyState();
    }
    public override void ApplyState()
    {
        rot = transform.eulerAngles;
        pos = transform.position;

        if (isCleaned)
        {
            pos.x = cleanXPos;
            rot.y = cleanYRot;

        }
        else
        {
            pos.x = messyXPos;
            rot.y = messyRot;

        }

        transform.position = pos;
        transform.eulerAngles = rot;

    }

    protected override void OnClicked()
    {

        if (!isCleaned)
        {
            //pos.x = cleanXPos;
            //transform.position = pos;

            //rot.y = cleanYRot;
            //transform.eulerAngles = rot;

            StartCoroutine(ChairMovement(cleanXPos, cleanYRot));
            amountCleaned++;
            UpdateAboutCleaning?.Invoke(amountCleaned);
            isCleaned = true;

        }



    }
    private IEnumerator ChairMovement(float targetXpos, float targetYRot)
    {
        Vector3 startpos = transform.position;
        Vector3 endpos = new Vector3(targetXpos, startpos.y, startpos.z);

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(startRot.eulerAngles.x,
            targetYRot,//change y rotation to correct
            startRot.eulerAngles.z //keep the same rotation
            );
        float elapsed = 0f;
        while (elapsed < animationTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationTime);

            transform.position = Vector3.Lerp(startpos, endpos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);

            yield return null;
        }
        transform.position = endpos;
        transform.rotation = endRot;
    }
}
