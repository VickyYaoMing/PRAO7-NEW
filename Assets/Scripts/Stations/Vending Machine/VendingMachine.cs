using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class VendingMachine : TaskBase
{
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private float vendSpeed = 1f;
    [SerializeField] private Transform[] itemSlots;

    private bool busy;

    private void Start()
    {
        //Timer.Create(.5f, true, false, false, "VendingMachine Timer").Timeout += Blink;
    }

    protected override void Update()
    {
        base.Update();

        // placeholder code to pick up item
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
            {
                if (hitInfo.rigidbody != null)
                {
                    OnMissionAccomplished();

                    Scene.Exit(true);
                }
            }
        }
    }

    public void Blink()
    {
        if (display.text.Length > 0 && display.text[^1] == '|')
            display.text = display.text.Substring(0, display.text.Length - 1);
        else
            display.text += '|';
    }

    public void Type(string s)
    {
        if (display.text.Length > 0 && display.text[^1] == '|')
            display.text = display.text.Substring(0, display.text.Length - 1);
        if (display.text.Length > 3) return;
        display.text += s;
    }

    public void Clear()
    {
        display.text = string.Empty;
    }

    public void Submit()
    {
        if (busy) return;

        if (display.text.Length > 0 && display.text[^1] == '|')
            display.text = display.text.Substring(0, display.text.Length - 1);

        int slot = -1;

        try
        {
            slot = Convert.ToUInt16(display.text);
        }
        catch
        {
            Clear();
            return;
        }

        slot -= 1;

        if (slot > -1 && slot < itemSlots.Length)
        {
            if (itemSlots[slot].childCount > 0)
            {
                Transform objectToVend = itemSlots[slot].GetChild(0);
                busy = true;
                StartCoroutine(Vend(objectToVend));
            }
        }

        Clear();
    }

    private IEnumerator Vend(Transform transform)
    {
        if (!transform.TryGetComponent(out Rigidbody rb))
            Debug.LogError("VendingMachine items need to have the rigidbody component!");

        rb.isKinematic = true;

        float delta = 0;
        while (transform.localPosition.z != 0.3f)
        {
            float z = transform.localPosition.z;
            z = Mathf.MoveTowards(z, 0.3f, Time.deltaTime * vendSpeed);
            delta = z - transform.localPosition.z;

            int thisIdx = transform.GetSiblingIndex();
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (i == thisIdx) continue;
                Transform sibling = transform.parent.GetChild(i);
                sibling.localPosition = sibling.localPosition.WithZ(sibling.localPosition.z + delta);
            }

            transform.localPosition = transform.localPosition.WithZ(z);
            yield return null;
        }

        yield return new WaitForFixedUpdate();

        transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddRelativeTorque(
            new Vector3(
                UnityEngine.Random.Range(-1, 1),
                UnityEngine.Random.Range(-1, 1),
                UnityEngine.Random.Range(-1, 1)
                )
            );
        busy = false;
    }
}
