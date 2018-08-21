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

export class Hand3D extends React.Component<Props, State> {
    scene: THREE.Scene | null = null;
    camera: THREE.PerspectiveCamera | null = null;
    renderer: THREE.WebGLRenderer | null = null;
    mesh1: THREE.Mesh | null = null;
    mesh2: THREE.Mesh | null = null;

    rotors: THREE.Mesh[] = [];

    public constructor(props: Props)
    {
        super(props);
        this.initScene();
        this.loadModel();
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

    axes() {
        // The X axis is red. The Y axis is green. The Z axis is blue.
        var axesHelper = new THREE.AxesHelper( 5 );
        this.scene.add( axesHelper );
    }

    async loadGeometry(url: string): Promise<THREE.Geometry> {
        var loader = new THREE.STLLoader();

        const promise = new Promise<THREE.Geometry>(
            (resolve, reject) => {
                loader.load(
                    // resource URL
                    url,
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

        this.axes();
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
        this.renderer.setSize( 800, 600 );

        this.camera.position.y = -10;
        this.camera.position.z = 10;
        const controls = new THREE.OrbitControls( this.camera, this.renderer.domElement );
        controls.target.set( 0, 0, 0 )
    }

    async loadModel()
    {
        const baseGeomtry = await this.loadGeometry('models/base.stl');
        const baseMaterial = new THREE.MeshPhongMaterial( { color: 0x3355FF, specular: 0x111111, shininess: 200 } );
        const base = new THREE.Mesh( baseGeomtry, baseMaterial );
        this.scene.add(base);

        var lastMesh: THREE.Mesh = null;

        for(let i = 0; i < 5; i++ ) {
            const mfGeometry = await this.loadGeometry('models/mf.stl');
            const mfMaterial = new THREE.MeshPhongMaterial( { color: 0x33FF33, specular: 0x111111, shininess: 200 } );
            const mf = new THREE.Mesh( mfGeometry, mfMaterial );
            
            if (lastMesh) {
                mf.translateY(-0.5);
                mf.translateZ(1.1);
                lastMesh.add( mf );
            }
            else {
                mf.translateZ(0.9);
                this.scene.add( mf );
            }

            let servo: THREE.Mesh = null;

            const servoGeomtry = await this.loadGeometry('models/MG996R.stl');
            const servoMaterial = new THREE.MeshPhongMaterial( { color: 0xff5533, specular: 0x111111, shininess: 200 } );
            servo = new THREE.Mesh( servoGeomtry, servoMaterial );
            servo.castShadow = true;
            servo.receiveShadow = true;
            servo.translateZ(0.2);
            mf.add( servo );

            const hornGeometry = await this.loadGeometry('models/Horn.stl');

            const hornMaterial = new THREE.MeshPhongMaterial( { color: 0x55FF33, specular: 0x111111, shininess: 200 } );
            const horn = new THREE.Mesh( hornGeometry, hornMaterial );
            horn.castShadow = true;
            horn.receiveShadow = true;
            horn.translateZ(1.3);
            horn.translateY(-0.6);
            servo.add(horn);

            this.rotors.push(horn);

            lastMesh = horn;
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

    animate()
    {
        if (this.mesh1 && this.mesh2) {
            this.mesh1.rotation.x += 0.01;
            this.mesh2.rotation.y += 0.02;
        }

        const values = this.props.rotorsDegrees;
        for(let i = 0; i < this.rotors.length; i++) {
            const rads = (values[i] || 0) * Math.PI / 180;
            this.rotors[i].rotation.z = rads;
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