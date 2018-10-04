var Example = Example || {};

Matter.use("matter-wrap");

Example.ballPool = function (config) {

  var Engine = Matter.Engine,
    Render = Matter.Render,
    Runner = Matter.Runner,
    Composite = Matter.Composite,
    Composites = Matter.Composites,
    Common = Matter.Common,
    MouseConstraint = Matter.MouseConstraint,
    Mouse = Matter.Mouse,
    World = Matter.World,
    Bodies = Matter.Bodies,
    Body = Matter.Body,
    Events = Matter.Events

  // create engine
  var engine = Engine.create(),
    world = engine.world;

    // 0 gravity
    world.gravity.x = 0;
    world.gravity.y = 0;

  var render = Render.create({
    element: document.body,
    canvas: config.canvas,
    engine: engine,
        options: {
            width: config.width,
            height: config.height,
            showVelocity: true,
            hasBounds: true,
            wireframes: false,
            background:"./img/grass-texture.jpg"
    }
  });

  Render.run(render);

  // create runner
  var runner = Runner.create();
  Runner.run(runner, engine);

  // add bodies
    var borderWidth = 15;
    var borderTop = Bodies.rectangle(config.width/2, 15, config.width*2, borderWidth, { isStatic: true });
    var borderBottom = Bodies.rectangle(config.width / 2, config.height - 15, config.width * 2, borderWidth, { isStatic: true });
    var borderLeft = Bodies.rectangle(10, config.width / 2, borderWidth, config.width * 2, { isStatic: true });
    var borderRight = Bodies.rectangle(config.width - 15, config.height / 2, borderWidth, config.width * 2, { isStatic: true });

    

    World.add(world, [
          borderTop,
          borderBottom,
          borderLeft,
          borderRight
    ]);

    if (!config.border.top) {
        World.remove(world, borderTop);
    }
    if (!config.border.right) {
        World.remove(world, borderRight);
    }
    if (!config.border.bottom) {
        World.remove(world, borderBottom);
    }
    if (!config.border.left) {
        World.remove(world, borderLeft);
    }
    

    var circule = Bodies.circle(config.width / 2, config.height/2, Common.random(15, 100), {
        restitution: 0.6,
        friction: 0.9,
        frictionAir: 0.01,
        render: {
            sprite: {
                texture: "./img/ball.png"
            }
        }
    });

    var stack = Composites.stack(1, 0, 1, 1, 1, 1, function (x, y) {
        
        return circule;
  });

  World.add(world, [stack]);

  

  var mouse = Mouse.create(render.canvas),
    mouseConstraint = MouseConstraint.create(engine, {
      mouse: mouse,
      constraint: {
        stiffness: 0.2,
        render: {
          visible: false
        }
      }
    });

    World.add(world, mouseConstraint);

    var currentPosition = {
        x: null,
        y: null
    };


    Events.on(engine, 'beforeUpdate', function (e) {

        if (config.externalMove) {
            //config.externalMove = false;
            
        } else {
            var x = parseInt(circule.position.x);
            var y = parseInt(circule.position.y);
            var itMoved = false;


            if (currentPosition.x !== x) {
                currentPosition.x = x;
                itMoved = true;
            }
            if (currentPosition.y !== y) {
                currentPosition.y = y;
                itMoved = true;
            }

            if (itMoved) {
                //console.log('beforeUpdate...x:' + x + ' y:' + y);
                config.moveCallback(currentPosition);
            }
        }
    })

    Events.on(mouseConstraint, "startdrag", function (e) {
        config.externalMove = false;
    })

  // keep the mouse in sync with rendering
  render.mouse = mouse;

  // fit the render viewport to the scene
  /*
  Render.lookAt(render, {
      min: { x: 0, y: 0 },
      max: { x: 800, y: 600 }
  });
  */


  // wrapping using matter-wrap plugin
  /*var allBodies = Composite.allBodies(world);

  for (var i = 0; i < allBodies.length; i += 1) {
    allBodies[i].plugin.wrap = {
      min: { x: render.bounds.min.x - 100, y: render.bounds.min.y },
      max: { x: render.bounds.max.x + 100, y: render.bounds.max.y }
    };
  }*/

  // context for MatterTools.Demo
  return {
    engine: engine,
    runner: runner,
    render: render,
    canvas: render.canvas,
    stop: function() {
      Matter.Render.stop(render);config.externalMove
      Matter.Runner.stop(runner);
      },
      move: function (x, y) {
          config.externalMove = true;
          Body.setPosition(circule, { x: x, y: y })
      },
      center: function () {
          config.externalMove = false;
          Body.setPosition(circule, { x: config.width / 2, y: config.height / 2});
      },
      redraw: function (mosaicItem) {
          config.externalMove = false;

          config.border.top = true;
          config.border.right = true;
          config.border.bottom = true;
          config.border.left = true;


          if (mosaicItem.hasANeighborLeft) {
              config.border.left = false;
              World.remove(world, borderLeft);
          } 

          if (mosaicItem.hasANeighborRight) {
              config.border.right = false;
              World.remove(world, borderRight);
          }
          
      }
  };
};



