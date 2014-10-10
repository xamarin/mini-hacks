# Cardboard F# Minihack

Open the FSSceneKit solution in Xamarin Studio.

Run it. Hey, that’s pretty good! Your first scene kit app in F#!

3D. Right. OK. Let’s see…

We’re going to need two views, side by side, right?

Go to the code where we define our view:

        //Configure view
        let r = new RectangleF(new PointF(0.0f, 0.0f), new SizeF(UIScreen.MainScreen.Bounds.Size))
        let s = new SCNView(r)
        configView s scene |> ignore
        this.View <- s

We’re going to want two of these `SCNView`s. We could just copy the code, but let’s be lazy programmers and avoid duplicating code. We can use F#’s pipe operator to operate on two side-by-side rectangles. Replace that code with the following:

        //Configure views
        let outer = new UIView(UIScreen.MainScreen.Bounds)

        let ss = 
            [
                new RectangleF(new PointF(0.0f, 0.0f), new SizeF(float32 UIScreen.MainScreen.Bounds.Width / 2.0f, UIScreen.MainScreen.Bounds.Height));
                new RectangleF(new PointF(float32 UIScreen.MainScreen.Bounds.Width / 2.0f, 0.0f), new SizeF(UIScreen.MainScreen.Bounds.Width / 2.0f, UIScreen.MainScreen.Bounds.Height));
            ]
            |> List.map (fun r -> new SCNView(r))
            |> List.map (fun s -> outer.AddSubview(configView s scene); s)

        this.View <- outer

Run it! Pretty good! Side-by-side `SCNView`s running the same view graph! But if you try this in Cardboard, you won’t get a 3D effect since both views have the exact same perspective. So we need to have our`SCNView`s use two different cameras.

Go to this code:

        //Cameras!
       let leftCameraNode = buildCamera scene (new SCNVector3 (0.0F, 9.0F, 50.0F))

       let targetNode = new SCNNode ()
       

And add another camera node to the scene — this time with a slightly different X location:

         //Cameras!
        let leftCameraNode = buildCamera scene (new SCNVector3 (0.0F, 9.0F, 50.0F))

        let rightCameraNode = buildCamera scene (new SCNVector3 (0.2F, 9.0F, 50.0F))

        let targetNode = new SCNNode ()

We don't want you to have to go cross-eyed, so comment out this code:

        //Point the camera
        let lc = SCNLookAtConstraint.Create (targetNode);
        leftCameraNode.Constraints <- [lc].ToArray().Cast<SCNConstraint>().ToArray()

OK, now we have to use this right-hand camera we’ve created in our right-hand view. Right before we write `this.View <- outer` add these few lines of code:

        //Shift camera in right
        ss.Head.PointOfView <- leftCameraNode
        ss.Tail.Head.PointOfView <- rightCameraNode
       
        this.View <- outer

To review what this is: our `ss` list contains two `SCNView`s: the one rendering on the left of the screen, and the one rendering on the right. We set their `PointOfView`s to the `SCNNode`s built by the `buildCamera` function, and we had called that function to place these camera nodes in two slightly different positions, but pointing in the same direction, just as eyes do.

Run the app! Even better, deploy it to an iPhone or iPod and view the 3D effect in Cardboard!

Bonus:

What fun is 3D without motion? Add the following code between the “Point the cameras” section and the “Configure views” section:

        // Action!

        let animation = new CABasicAnimation(
            KeyPath = "rotation"
        )
        let t = new SCNVector4 (1.0F, 1.0F, 0.0F, float32 (Math.PI * 2.0))
        animation.To <- NSValue.FromVector (t)
 
        animation.Duration <- float 5.0F
        animation.RepeatCount <- float32 Double.MaxValue //repeat forever
       
        bs |> List.iter (fun b -> b.AddAnimation(animation,new NSString("rotation")))

Congratulations on your animated 3D SceneKit app programmed in F#!        