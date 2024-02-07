using JetBrains.Annotations;
using UnityEngine;
using Zenject;

public class CanvasController
{
    [Inject, UsedImplicitly] public OverlayCanvas OverlayCanvasCanvas { get; }
    [Inject, UsedImplicitly] public CameraCanvas CameraCanvas { get; }

    public CanvasType DefaultCanvasType => CanvasType.Overlay;

    public Canvas Get(CanvasType type = CanvasType.Default)
    {
        if (type == CanvasType.Default) type = DefaultCanvasType;
        return type == CanvasType.Overlay ? OverlayCanvasCanvas.Canvas : CameraCanvas.Canvas;
    }
    
}
