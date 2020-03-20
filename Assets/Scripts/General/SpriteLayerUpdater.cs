using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerUpdater : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] int[] spriteRendererOffsets;
    [SerializeField] bool objectNotMoving = false;
    [SerializeField] bool useParent = false;
    [SerializeField] Transform transformReference;

    // Start is called before the first frame update
    void Start()
    {
        if (useParent)
        {
            transformReference = GetComponentInParent<Transform>();
        }
        else
        {
            transformReference = GetComponent<Transform>();
        }
        
        UpdateSpriteLayer();
    }

    private void Update()
    {
        if (objectNotMoving) return;
        UpdateSpriteLayer();
    }

    private void UpdateSpriteLayer()
    {
        if (spriteRenderers[0] == null) return;
        for(int i=0; i<spriteRenderers.Length; i++)
        {
            int offset = 0;
            if(spriteRendererOffsets != null)
            {
                offset = spriteRendererOffsets[i];
            }
            //spriteRenderers[i].sortingOrder = Mathf.RoundToInt(transformParent.position.y * 100f) * -1;
            int pos = Mathf.RoundToInt(transformReference.transform.position.y * 10);
            //pos /= 3;
            spriteRenderers[i].sortingOrder = (pos * -1) + offset;
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        UpdateSpriteLayer();
    }
}
