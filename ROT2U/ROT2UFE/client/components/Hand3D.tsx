import * as React from 'react';
import {RouteComponentProps} from "react-router";
import * as ReactDOM from 'react-dom';
import * as THREE from 'three';
import "three/examples/js/controls/OrbitControls.js";
import "three/examples/js/loaders/OBJLoader.js";
import "three/examples/js/loaders/STLLoader.js";
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';

interface Props {
    rotorsDegrees: number[]
}

interface State { 
}

interface IServoInfo {
    servo: THREE.Mesh;
    horn: THREE.Mesh;
    top: THREE.Group;
}

interface IGraspInfo {
    grasper: THREE.Object3D;
    right_rotor: THREE.Object3D;
    left_rotor: THREE.Object3D;
}

export class Hand3D extends React.Component<Props, State> {
    scene: THREE.Scene | null = null;
    camera: THREE.PerspectiveCamera | null = null;
    renderer: THREE.WebGLRenderer | null = null;
    mesh1: THREE.Mesh | null = null;
    mesh2: THREE.Mesh | null = null;

    rotors: THREE.Object3D[][] = [];

    public constructor(props: Props)
    {
        super(props);
        this.initScene();
        //this.loadModel();
        this.loadArm();
    }

    addShadowedLight( x, y, z, color, intensity ) {

        var directionalLight = new THREE.DirectionalLight( color, intensity );
        directionalLight.position.set( x, y, z );
        this.scene.add( directionalLight );

        directionalLight.castShadow = true;

        var d = 1;
        directionalLight.shadow.camera.left = -d;
        directionalLight.shadow.camera.right = d;
        directionalLight.shadow.camera.top = d;
        directionalLight.shadow.camera.bottom = -d;

        directionalLight.shadow.camera.near = 1;
        directionalLight.shadow.camera.far = 4;

        directionalLight.shadow.mapSize.width = 1024;
        directionalLight.shadow.mapSize.height = 1024;

        directionalLight.shadow.bias = -0.002;

    }

    axes(target: THREE.Object3D) {
        // The X axis is red. The Y axis is green. The Z axis is blue.
        var axesHelper = new THREE.AxesHelper( 5 );
        target.add( axesHelper );
    }

    async loadGeometry(url: string): Promise<THREE.Geometry> {
        var loader = new THREE.STLLoader();

        const promise = new Promise<THREE.Geometry>(
            (resolve, reject) => {
                loader.load(
                    // resource URL
                    url + "?" + Date.now(),
                    // called when resource is loaded
                    ( geometry ) => {
                        resolve(geometry);
                    },
                    // called when loading is in progresses
                    (xhr) => console.log( ( xhr.loaded / xhr.total * 100 ) + '% loaded' ),
                    // called when loading has errors
                    (error) => reject(error)
                );
            }
        );
        
        return promise;
    }

    initScene() {
        this.scene = new THREE.Scene();

        this.axes(this.scene);
        //this.scene.background = new THREE.Color( 0x72645b );
        //this.scene.fog = new THREE.Fog( 0x72645b, 2, 15 );

        this.camera = new THREE.PerspectiveCamera( 75, window.innerWidth / window.innerHeight, 1, 10000 );
        this.camera.up = new THREE.Vector3(0, 0, 1);

        // Lights
        this.scene.add( new THREE.HemisphereLight( 0x443333, 0x111122 ) );
        this.addShadowedLight( 400, -500, 700, 0xffffff, 1.35 );
        //this.addShadowedLight( 0.5, 1, -1, 0xffaa00, 1 );


        // Ground

        var plane = new THREE.Mesh(
            new THREE.PlaneBufferGeometry( 40, 40 ),
            new THREE.MeshPhongMaterial( { color: 0x999999, specular: 0x101010 } )
        );
        //plane.rotation.x = -Math.PI/2;
        //plane.position.y = -0.5;
        this.scene.add( plane );

        plane.receiveShadow = true;


        var geometry = new THREE.BoxGeometry( 200, 200, 200 );
        var material = new THREE.MeshBasicMaterial( { color: 0xff0000, wireframe: true } );
    
        var material2 = new THREE.MeshBasicMaterial( { color: 0xff0000 } );
        this.mesh1 = new THREE.Mesh( geometry, material );
        this.mesh2 = new THREE.Mesh( geometry, material );

        const c = new THREE.CylinderGeometry(100, 100, 100, 36);
        const cMesh = new THREE.Mesh(c, material);

        //this.scene.add(cMesh);

        this.mesh1.translateX(-200);
        this.mesh2.translateX(200);
        this.mesh1.add( this.mesh2 );

        //this.scene.add( this.mesh1 );

        this.renderer = new THREE.WebGLRenderer();
        //this.renderer.setSize( window.innerWidth, window.innerHeight );
        this.renderer.setSize( 1200, 900 );

        this.camera.position.y = -10;
        this.camera.position.z = 10;
        const controls = new THREE.OrbitControls( this.camera, this.renderer.domElement );

        // wasd control for pan
        controls.keys = {
            LEFT: 65, // a
            UP: 87, // w
            RIGHT: 68, // d
            BOTTOM: 83 // s
        }
        controls.target.set( 0, 0, 0 )
    }

    async loadArm() 
    {
        // bunch of magic numbers
        let last: THREE.Object3D = null;

        const baseGeomtry = await this.loadGeometry('models/base.stl');
        const baseMaterial = new THREE.MeshPhongMaterial( { color: 0x3355FF, specular: 0x111111, shininess: 200 } );
        const base = new THREE.Mesh( baseGeomtry, baseMaterial );
        base.translateZ(0.7);
        this.scene.add(base);

        const top = new THREE.Group();
        top.translateZ(1.4);
        base.add(top);

        const mf1 = await this.loadMF();
        mf1.translateY(1);
        top.add(mf1);
        const mf1top = new THREE.Group();
        mf1top.translateZ(0.2);
        mf1.add(mf1top);

        const servo1 = await this.loadServo();
        mf1top.add(servo1.servo);
        this.rotors.push([servo1.horn]);

        const mf2 = await this.loadMF();
        mf2.translateY(0.6);
        mf2.translateZ(0.2);
        mf2.rotateY(this.toRadians(-90));
        servo1.top.add(mf2);
        const mf2top = new THREE.Group();
        mf2top.translateZ(0.2);
        mf2.add(mf2top);

        const servo2 = await this.loadServo();
        mf2top.add(servo2.servo);
        this.rotors.push([servo2.horn]);

        const uBracket = await this.loadU();
        const uBracketTop = new THREE.Group();
        uBracket.add(uBracketTop);

        uBracket.translateZ(-1.8);
        uBracket.translateY(-2.5);
        uBracket.rotateX(this.toRadians(-90));
        servo2.top.add(uBracket);

        uBracketTop.rotateX(this.toRadians(180));
        this.axes(uBracketTop);

        const servo3 = await this.loadServo();
        servo3.servo.translateX(-1.5);
        servo3.servo.translateY(1);
        servo3.servo.translateZ(1);
        servo3.servo.rotateY(this.toRadians(90));
        uBracketTop.add(servo3.servo);

        const grasper = await this.loadGrasper();
        uBracketTop.add(grasper.grasper);

        this.rotors.push([servo3.horn, grasper.right_rotor, grasper.left_rotor]);
    }

    async loadU(): Promise<THREE.Mesh> {
        const mfGeometry = await this.loadGeometry('models/u.stl');
        const mfMaterial = new THREE.MeshPhongMaterial( { color: 0x55FF55, specular: 0x111111, shininess: 200 } );
        const mf = new THREE.Mesh( mfGeometry, mfMaterial );
        mf.translateZ(0.2);

        return mf;

    }

    async loadMesh(url: string, color: number): Promise<THREE.Mesh> {
        const bGeomtry = await this.loadGeometry(url);
        const bMaterial = new THREE.MeshPhongMaterial( { color: color, specular: 0x111111, shininess: 200 } );
        const base = new THREE.Mesh( bGeomtry, bMaterial ); 
        return base;
    }

    async loadGrasper(): Promise<IGraspInfo> {
        const bGeomtry = await this.loadGeometry('models/grasper_base.stl');
        const bMaterial = new THREE.MeshPhongMaterial( { color: 0xeeeeee, specular: 0x111111, shininess: 200 } );
        const base = new THREE.Mesh( bGeomtry, bMaterial ); 

        const group = new THREE.Group();

        const right_rotor = new THREE.Group();
        right_rotor.translateZ(0.8);
        right_rotor.translateY(0.1);
        right_rotor.translateX(0.05);
        right_rotor.rotateY(this.toRadians(-90));
        this.axes(right_rotor);

        const right_jaw = await this.loadMesh('models/grasper_jaw.stl', 0x005555);
        right_rotor.add(right_jaw);

        const left_rotor = new THREE.Group();
        left_rotor.translateZ(0.8);
        left_rotor.translateY(-0.1);
        left_rotor.translateX(0.05);
        left_rotor.rotateY(this.toRadians(90));
        this.axes(left_rotor);

        const left_jaw = await this.loadMesh('models/grasper_jaw.stl', 0x005555);
        left_jaw.rotateZ(this.toRadians(180));
        left_rotor.add(left_jaw);

        group.add(base);
        group.add(right_rotor);
        group.add(left_rotor);

        return {
            grasper: group,
            right_rotor,
            left_rotor,
        };
    }

    async loadMF(): Promise<THREE.Mesh> {
        const mfGeometry = await this.loadGeometry('models/mf.stl');
        const mfMaterial = new THREE.MeshPhongMaterial( { color: 0x33FF33, specular: 0x111111, shininess: 200 } );
        const mf = new THREE.Mesh( mfGeometry, mfMaterial );
        mf.translateZ(0.2);

        return mf;
    }

    async loadServo(): Promise<IServoInfo> {
        let servo: THREE.Mesh = null;

        const servoGeomtry = await this.loadGeometry('models/MG996R.stl');
        const servoMaterial = new THREE.MeshPhongMaterial( { color: 0xff5533, specular: 0x111111, shininess: 200 } );
        servo = new THREE.Mesh( servoGeomtry, servoMaterial );
        servo.castShadow = true;
        servo.receiveShadow = true;
        servo.translateZ(0.2);

        const hornGeometry = await this.loadGeometry('models/Horn.stl');

        const hornMaterial = new THREE.MeshPhongMaterial( { color: 0x55FF33, specular: 0x111111, shininess: 200 } );
        const horn = new THREE.Mesh( hornGeometry, hornMaterial );
        horn.castShadow = true;
        horn.receiveShadow = true;
        horn.translateZ(1.3);
        horn.translateY(-0.6);
        servo.add(horn);

        const top = new THREE.Group();
        top.translateZ(0.4);
        horn.add(top);

        this.axes(top);

        return {
            servo,
            horn,
            top
        }
    }

    componentDidMount()
    {
        const thisNode = ReactDOM.findDOMNode(this);
        if (this.renderer && thisNode) {
            thisNode.appendChild(this.renderer.domElement);
            requestAnimationFrame( this.animate.bind(this) );
        }
    }

    toRadians(degrees: number): number {
        return (degrees || 0) * Math.PI / 180;
    }

    animate()
    {
        if (this.mesh1 && this.mesh2) {
            this.mesh1.rotation.x += 0.01;
            this.mesh2.rotation.y += 0.02;
        }

        const values = this.props.rotorsDegrees;
        for(let i = 0; i < this.rotors.length; i++) {
            let value = values[i];
            switch(i) {
                case 2:
                    value = 180 - value;
                    break;
            }
            const rads = this.toRadians(value);
            this.rotors[i].forEach(r => r.rotation.z = rads);
        }

        requestAnimationFrame( this.animate.bind(this) );
        this.forceUpdate();
    }

    public render() {
        if (this.renderer && this.scene && this.camera) {
            this.renderer.render( this.scene, this.camera );
        }
        return <div></div>
    }
}