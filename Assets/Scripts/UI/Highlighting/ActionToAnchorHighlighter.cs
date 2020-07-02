using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionToAnchorHighlighter : MonoBehaviour
{
	private LineRenderer _lineRenderer;

	void Awake()
	{
        _lineRenderer = this.GetComponent<LineRenderer>();
        Debug.Assert(_lineRenderer != null, "Failed to find linerenderer component");
    }
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
