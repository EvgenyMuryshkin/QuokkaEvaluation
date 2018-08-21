import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as WeatherForecastsState from '../store/WeatherForecasts';
import * as Rx from "rxjs";
import axios from "axios";
import { debounceTime } from 'rxjs/operators';
import { ReportDTO, ImageDTO } from '../dto/report';
import * as Webcam from "react-webcam";
import { IFaceAPIResponse } from '../dto/face-api';
import { Hand3D } from './Hand3D';

console.log(Webcam);

// At runtime, Redux will merge together...
type WeatherForecastProps =
    WeatherForecastsState.WeatherForecastsState        // ... state we've requested from the Redux store
    & typeof WeatherForecastsState.actionCreators      // ... plus action creators we've requested
    & RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters


enum eMode {

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
}
class LED extends React.Component<WeatherForecastProps, IState> {
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

    constructor(props: WeatherForecastProps) {
        super(props);

        this.state = {
            foregroundColor: "#FFFFFF",
            backgroundColor: "#000000",
            colors: this.defaultColors(),
            face: null,
            emotion: "neutral",
            servos: [0,0,0,0,0]
        }

        this.subscription = this.tasks.pipe(debounceTime(100)).subscribe(_ => this.onReport());
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

        //this.tasks.next();
    }
    

    componentWillMount() {
        // This method runs when the component is first added to the page
        //let startDateIndex = parseInt(this.props.match.params.startDateIndex) || 0;
        //this.props.requestWeatherForecasts(startDateIndex);
        this.tasks.next();
    }

    componentWillReceiveProps(nextProps: WeatherForecastProps) {
        // This method runs when incoming props (e.g., route params) change
        //let startDateIndex = parseInt(nextProps.match.params.startDateIndex) || 0;
        //this.props.requestWeatherForecasts(startDateIndex);
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
    public render() {
        const {
            foregroundColor, 
            backgroundColor, 
            colors,
            face,
            emotion
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

export default connect(
    (state: ApplicationState) => state.weatherForecasts, // Selects which state properties are merged into the component's props
    WeatherForecastsState.actionCreators                 // Selects which action creators are merged into the component's props
)(LED);
