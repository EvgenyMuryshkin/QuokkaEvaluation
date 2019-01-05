import * as React from 'react';
import { connect } from 'react-redux';
import * as Rx from "rxjs";
import axios from "axios";
import { debounceTime } from 'rxjs/operators';
import { ReportDTO, ImageDTO } from '../dto/report';
import * as Webcam from "react-webcam";
import { IFaceAPIResponse } from '../dto/face-api';
import { Hand3D } from './Hand3D';
import { IStoryboard, Storyboard } from './storyboard';

interface IProps {

}
interface IEmotionPicture {
    color: string;
    rows: string[];
}
interface IState {
    foregroundColor: string;
    backgroundColor: string;
    colors: {[key: string]: string[]};
    face: IFaceAPIResponse | null;
    emotion: string;
    servos: number[];
    storyboards: IStoryboard[];
    selectedStoryboardIdx: number;
}
class LED extends React.Component<IProps, IState> {
    tasks = new Rx.Subject<{}>();
    subscription: Rx.Subscription;
    emotions = [
        "anger", 
        "contempt",
        "disgust",
        "fear",
        "happiness",
        "neutral",
        "sadness",
        "surprise",
    ];

    constructor(props: IProps) {
        super(props);

        this.state = {
            foregroundColor: "#FFFFFF",
            backgroundColor: "#000000",
            colors: this.defaultColors(),
            face: null,
            emotion: "neutral",
            servos: [0,0,180,0,0],
            storyboards: this.defaultStoryboards(),
            selectedStoryboardIdx: null
        }

        this.subscription = this.tasks.pipe(debounceTime(100)).subscribe(_ => this.onReport());
    }

    defaultStoryboards(): IStoryboard[] {
        const closed = 170;
        const opened = 120;

        const azimuth = 20;
        return [
            {
                name: "Rotate to 180",
                Items: [{
                    servos: [180, 0, closed, 0, 0]
                }]
            },
            {
                name: "Rotate to 0",
                Items: [{
                    servos: [0, 0, closed, 0, 0]
                }]
            },
            {
                name: "Idle",
                Items: [{
                    servos: [0, 90, opened, 0, 0]
                }]
            },
            {
                name: "Pick",
                Items: [
                    {
                        servos: [0, azimuth, opened, 0, 0]
                    },
                    {
                        servos: [0, 0, opened, 0, 0]
                    },
                    {
                        servos: [0, 0, closed, 0, 0]
                    },
                    {
                        servos: [0, azimuth, closed, 0, 0]
                    }
                ]
            },
            {
                name: "Deliver",
                Items: [
                    {
                        servos: [0, azimuth, closed, 0, 0]
                    },
                    {
                        servos: [180, azimuth, closed, 0, 0]
                    }
                ]
            },
            {
                name: "Release",
                Items: [
                    {
                        servos: [180, azimuth, closed, 0, 0]
                    },
                    {
                        servos: [180, 0, closed, 0, 0]
                    },
                    {
                        servos: [180, 0, opened, 0, 0]
                    },
                    {
                        servos: [180, azimuth, opened, 0, 0]
                    }
                ]
            },
        ]
    }

    defaultEmotions: {[key:string]: IEmotionPicture} = {
        "neutral": {
            color: "#0000FF",
            rows: [
                "00000000",
                "11100111",
                "00000000",
                "00011000",
                "00011000",
                "00000000",
                "01111110",
                "00000000",
            ]
        },
        "happiness": {
            color: "#00FF00",
            rows: [
                "10100101",
                "01000010",
                "00000000",
                "00011000",
                "00011000",
                "01000010",
                "00111100",
                "00000000",
            ]
        },
        "disgust":  {
            color: "#FF0000",
            rows: [
                "01000010",
                "10100101",
                "00000000",
                "00011000",
                "00011000",
                "00000000",
                "00111100",
                "01000010",
            ]
        },
    }

    selectMany<T>(items: Array<T>[]) : T[] {
        let merged: T[] = [];
        for(var item of items.filter(i => i))  {
            merged = merged.concat(item);
        }
        
        return merged;
    }

    emotionToArray(picture: IEmotionPicture): string[] {
        const rowColors = picture.rows.map(row => {
            const rowColor = [...Array(row.length).keys()]
                .map(idx => row[idx])
                .map(c => c == "0" ? "#000000" : picture.color);

            return rowColor;
        });

        return this.selectMany(rowColors);
    }

    defaultColors() : {[key: string]: string[]} {
        return this.emotions.reduce( (p,c) => {
            if (this.defaultEmotions[c]) {
                p[c] = this.emotionToArray(this.defaultEmotions[c]);
            }
            else {
                p[c] = [...Array(64).keys()].map(pos => "#000000");
            }
            return p;
        }, {});
    }

    toColor(value: string): number {
        return Math.min(1, parseInt(value, 16));
    }

    async delay(ms: number) {
        await Rx.timer(ms).toPromise();
    }

    async runStoryboard(storyboard: IStoryboard) {
        const sb = new Storyboard();
        await sb.run(
            storyboard, 
            () => this.state.servos, 
            (data) => this.setState({
                servos: data
            }))
    }

    async onReport() {
        const {colors, emotion, servos} = this.state;

        const dto = new ReportDTO();
        dto.colors = colors[emotion].map(c => {
            const r = this.toColor(c.substring(1, 3));
            const g = this.toColor(c.substring(3, 5));
            const b = this.toColor(c.substring(5, 7));
            return (r << 16) | (g << 8) | b;
        });

        dto.servos = servos;

        await axios.request({
            method: "post",
            url: "/api/Report/Colors",
            data: dto
        });

        this.tasks.next();
    }
    

    componentWillMount() {
        // TODO: load state from 
        this.tasks.next();
    }

    componentWillReceiveProps(nextProps: IProps) {
    }

    servosControl() {
        const {servos} = this.state;
        const controls = servos.map((v,idx) => {
            return <div key={idx}>
                <span style={{fontSize: "xx-large", display: "block"}}>Servo: {idx} ({v})</span>
                <input type="range" min={0} max={180} value={v} onChange={e => {
                    const newServos = [...servos];
                    newServos[idx] = parseInt(e.target.value);
                    this.setState({
                        servos: newServos
                    })
                }}/>
            </div>
        })
        return <div>{controls}</div>
    }

    async runScenario() {
        const {storyboards} = this.state;

        const steps = ["Pick", "Deliver", "Release", "Idle"];

        for (const s of steps) {
            const idx = storyboards.findIndex(sb => sb.name == s);
            if (idx == -1) {
                alert(`Storyboard ${s} not found`);
                return;
            }

            const storyboard = storyboards[idx];
            this.setState({
                selectedStoryboardIdx: idx
            });

            await this.runStoryboard(storyboard);
        }
    }

    public render() {
        const {
            foregroundColor, 
            backgroundColor, 
            colors,
            face,
            emotion,
            storyboards,
            selectedStoryboardIdx
        } = this.state;

        const setColor = (pos: number, color: string) => {
            const newColors = [...colors[emotion]];
            newColors[pos] = color;
            this.setState({
                colors: Object.assign(colors, {[emotion]: newColors})
            }) 
        }

        const getColor = (pos: number) => {
            return colors[emotion][pos];
        }

        const dims = [...Array(8).keys()];
        const rows = dims.map(r => {
            return <tr key={r}>
                {
                    dims.map(c => {
                        const pos = dims.length * r + c;
                        const discardHandler = (e: React.MouseEvent<HTMLDivElement>) => {
                            e.preventDefault();
                        }

                        const clickHandler = (e: React.MouseEvent<HTMLDivElement>) => {
                            switch(e.buttons) {
                                case 1:
                                    setColor(pos, foregroundColor);                                    
                                    break;
                                case 2:
                                    setColor(pos, backgroundColor);                                    
                                    break;
                            }

                            discardHandler(e);
                        }
                  
                        const tdStyle: React.CSSProperties = {
                            background: getColor(pos)
                        };

                        return <td key={`${r}_${c}`} className="led-cell"> 
                            <div className="led-cell-color"
                                style={tdStyle}
                                onContextMenu={discardHandler}  
                                onMouseOver={clickHandler}
                                onMouseUp={discardHandler}
                                onMouseDown={clickHandler}>
                            </div>
                        </td>
                    })
                }
            </tr>
        })

        /*
        navigator.mediaDevices
            .enumerateDevices()
            .then(devices => console.log(devices.filter(d => d.kind === "videoinput" )))

        navigator.mediaDevices.getUserMedia({
            video: {
                deviceId: "a83e6286959ad30929fb609ccb444862ff00231b9590b55e3d764d6517e5b3e2"
            },
        }).then(c => {
            console.log(c);
        })
    

        navigator.mediaDevices.getUserMedia({
            video: {
                deviceId: "8948c4a9a163d604888135b82bc713c651cbc45d32c040a181062b2400f164de"
            },
        }).then(c => {
            console.log(c);
        })
        */
       
        return <div>
            <div>
                <input className="color-picker" type="color" value={foregroundColor} onChange={e => {
                    this.setState({
                        foregroundColor: e.target.value
                    });
                }}/>
                <input className="color-picker" type="color" value={backgroundColor} onChange={e => {
                    this.setState({
                        backgroundColor: e.target.value
                    });
                }}/>
            </div>
            <div>
                <select value={emotion} onChange={e => this.setState({emotion: this.emotions[e.target.selectedIndex]})}>
                    {this.emotions.map(e => <option key={e} value={e}>{e}</option>)}
                </select>
            </div>
            <table className="led-table">
                <tbody>
                    {rows}
                </tbody>
            </table>
            <div>
                <button onClick={e => this.takePicture()}>Photo</button>
            </div>
            { face && <div>
                    <div>Age: {face.faceAttributes.age}</div>
                    <div>Emotion: {emotion} ({face.faceAttributes.emotion[emotion]})</div>
                </div>
            }

            { false && <Webcam 
                ref={r => this.setRef(r)}
                audio={false}
                height={400}
                screenshotFormat="image/jpeg"
                width={535}
                videoConstraints={
                    {
                        deviceId: "a83e6286959ad30929fb609ccb444862ff00231b9590b55e3d764d6517e5b3e2"
                    }
                }
            />
            }
            { false && <Webcam 
                ref={r => this.setRef(r)}
                audio={false}
                height={400}
                screenshotFormat="image/jpeg"
                width={535}
                videoConstraints={
                    {
                        deviceId: "8948c4a9a163d604888135b82bc713c651cbc45d32c040a181062b2400f164de"
                    }
                }
            /> }

            {this.servosControl()}
            <select value={selectedStoryboardIdx} 
                onChange={e => this.setState({
                    selectedStoryboardIdx: e.target.selectedIndex > 0 ? e.target.selectedIndex - 1 : null
                })}>
                <option value={null}>Select storyboard...</option>
                {storyboards.map((s, idx) => <option key={s.name} value={idx}>{s.name}</option>)}
            </select>
            <button onClick={e => this.runStoryboard(this.state.storyboards[selectedStoryboardIdx])}>Run</button>
            <button onClick={e => this.runScenario()}>Run scenario</button>
            <Hand3D rotorsDegrees={this.state.servos}/>
        </div>;
    }

    public setRef = (webcam: any) => {
        this.webcam = webcam;
    }
    webcam: any;

    async takePicture() {
        const imageSrc: string = this.webcam.getScreenshot();   
        const base64 = imageSrc.substring(imageSrc.indexOf(",") + 1);
        const dto = new ImageDTO();
        dto.Base64Image = base64;
        
        const response = await axios.request({
            method: "post",
            url: "/api/Report/Image",
            data: dto
        });

        const apiResponse: IFaceAPIResponse[] = response.data;

        console.log(apiResponse);

        const face = apiResponse[0];
        if (face) {
            const keys = Object.keys(face.faceAttributes.emotion);
            const winner = keys.reduce((p,c) => {
                if (face.faceAttributes.emotion[c] > face.faceAttributes.emotion[p]) {
                    return c;
                }

                return p;
            }, "neutral");

            this.setState({
                face: face,
                emotion: winner
            })
        }
    }
}

export default connect()(LED);
