import * as React from 'react';
import {RouteComponentProps} from "react-router";
import * as ReactDOM from 'react-dom';
import * as Three from 'three';

interface Props {
}

interface State { 
}

export class Hand3D extends React.Component<Props, State> {
    scene: Three.Scene | null = null;
    camera: Three.PerspectiveCamera | null = null;
    renderer: Three.WebGLRenderer | null = null;
    mesh1: Three.Mesh | null = null;
    mesh2: Three.Mesh | null = null;

    public constructor(props: Props)
    {
        super(props);
        this.init();
    }

    init()
    {
        this.scene = new Three.Scene();
    
        this.camera = new Three.PerspectiveCamera( 75, window.innerWidth / window.innerHeight, 1, 10000 );
        this.camera.position.z = 1000;
    
        var geometry = new Three.BoxGeometry( 200, 200, 200 );
        var material = new Three.MeshBasicMaterial( { color: 0xff0000, wireframe: true } );
    
        this.mesh1 = new Three.Mesh( geometry, material );
        this.mesh2 = new Three.Mesh( geometry, material );

        this.mesh1.translateX(-200);
        this.mesh2.translateX(200);

        this.scene.add( this.mesh1 );
        this.scene.add( this.mesh2 );
    
        this.renderer = new Three.WebGLRenderer();
        this.renderer.setSize( window.innerWidth, window.innerHeight );
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
            this.mesh1.rotation.y += 0.02;
    
            requestAnimationFrame( this.animate.bind(this) );
            this.forceUpdate();
        }
    }

    public render() {
        if (this.renderer && this.scene && this.camera) {
            this.renderer.render( this.scene, this.camera );
        }
        return <div></div>
    }
}