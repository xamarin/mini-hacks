namespace SceneKitFSharp
 
open System
open System.IO
open System.Linq
open FSharp.Data
open MonoTouch.UIKit
open MonoTouch.Foundation
open MonoTouch.SceneKit
open MonoTouch.CoreAnimation
open System.Drawing

[<Register("FSSceneKitViewController")>]
type FSSceneKitViewController() = 
    inherit UIViewController()
 
    let building width length height posx posy (scene:SCNScene) (rnd:Random) =
        let boxNode = new SCNNode ()
        boxNode.Geometry <- new SCNBox(
            Width = width, 
            Height = height, 
            Length = length, 
            ChamferRadius = 0.02f
        )
        boxNode.Position <- new SCNVector3(posx, height/2.0F, posy)

        scene.RootNode.AddChildNode (boxNode)
        let buildings = ["Content/building1.jpg"; "Content/building2.jpg"; "Content/building3.jpg"]
        let material = new SCNMaterial ()
        material.Diffuse.Contents <- UIImage.FromFile (buildings.[rnd.Next(buildings.Length)])
        material.Diffuse.ContentsTransform <- SCNMatrix4.Scale ( new SCNVector3(width,height,1.F))
        material.Diffuse.WrapS <- SCNWrapMode.Repeat
        material.Diffuse.WrapT <- SCNWrapMode.Repeat
        material.Diffuse.MipFilter <- SCNFilterMode.Linear
        material.Diffuse.MinificationFilter <- SCNFilterMode.Linear
        material.Diffuse.MagnificationFilter <- SCNFilterMode.Linear
        material.Specular.Contents <- UIColor.Gray

        material.LocksAmbientWithDiffuse <- true

        boxNode.Geometry.FirstMaterial <- material
        boxNode

    let random (min, max, (rnd:Random)) =
        float32 (rnd.Next(min, max))

    let buildCamera (scene : SCNScene) loc =
        let c = new SCNNode()
        c.Camera <- new SCNCamera()
        scene.RootNode.AddChildNode (c)
        c.Position <- loc
        c

    let configView (view : SCNView) scene =
        view.ClipsToBounds <- true
        view.Scene <- scene
        view.AllowsCameraControl <- false
        view.ShowsStatistics <- false
        view.BackgroundColor <- UIColor.FromRGB(52, 152, 219)
        view

    override this.ViewDidLoad () =
        let scene = new SCNScene ()

        //Positions everyone!
        let rnd = new Random();

        let bs = List.map (fun i -> (building 
                                    (random (1, 5, rnd)) 
                                    (random (1, 5, rnd)) 
                                    (random (1, 10, rnd)) 
                                    (random (-50, 50, rnd)) 
                                    (random (-50, 50, rnd))
                                    scene 
                                    rnd)) [0..200] 

        //Lights!
        let lightNode = new SCNNode()
        lightNode.Light <- new SCNLight ()
        lightNode.Light.LightType <- SCNLightType.Omni
        lightNode.Position <- new SCNVector3 (30.0F, 20.0F, 60.0F)
        scene.RootNode.AddChildNode (lightNode)
 
        let ambientLightNode = new SCNNode ()
        ambientLightNode.Light <- new SCNLight ()
        ambientLightNode.Light.LightType <- SCNLightType.Ambient
        ambientLightNode.Light.Color <- UIColor.DarkGray
        scene.RootNode.AddChildNode (ambientLightNode)
 
        //Cameras!
        let leftCameraNode = buildCamera scene (new SCNVector3 (0.0F, 9.0F, 50.0F))

        let targetNode = new SCNNode ()
        targetNode.Position <- new SCNVector3 (00.0F, 0.0F, 20.0F);
        scene.RootNode.AddChildNode (targetNode)

        //Point the camera
        let lc = SCNLookAtConstraint.Create (targetNode);
        leftCameraNode.Constraints <- [lc].ToArray().Cast<SCNConstraint>().ToArray()

        //Configure view
        let r = new RectangleF(new PointF(0.0f, 0.0f), new SizeF(UIScreen.MainScreen.Bounds.Size))
        let s = new SCNView(r)
        configView s scene |> ignore
        this.View <- s
 
[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit UIApplicationDelegate ()
 
    let mutable window:UIWindow = null

    // This method is invoked when the application is ready to run.
    override this.FinishedLaunching (app, options) =
        window <- new UIWindow (UIScreen.MainScreen.Bounds)
        window.RootViewController <- new FSSceneKitViewController()
        window.MakeKeyAndVisible ()
        true
 
module Main =
    [<EntryPoint>]
    let main args =
        UIApplication.Main (args, null, "AppDelegate")
        0