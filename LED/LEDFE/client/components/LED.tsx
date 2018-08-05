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

console.log(Webcam);

// At runtime, Redux will merge together...
type WeatherForecastProps =
    WeatherForecastsState.WeatherForecastsState        // ... state we've requested from the Redux store
    & typeof WeatherForecastsState.actionCreators      // ... plus action creators we've requested
    & RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters


enum eMode {

}
interface IState {
    foregroundColor: string;
    backgroundColor: string;
    colors: string[];
    face: IFaceAPIResponse | null;
    emotion: string;
}
class LED extends React.Component<WeatherForecastProps, IState> {
    tasks = new Rx.Subject<{}>();
    subscription: Rx.Subscription;

    constructor(props: WeatherForecastProps) {
        super(props);
        this.state = {
            foregroundColor: "#FFFFFF",
            backgroundColor: "#000000",
            colors: [...Array(64).keys()].map(pos => "#000000"),
            face: null,
            emotion: ""
        }

        this.subscription = this.tasks.pipe(debounceTime(100)).subscribe(_ => this.onReport());
    }

    toColor(value: string): number {
        return Math.min(1, parseInt(value, 16));
    }

    async onReport() {
        /*
        const dto = new ReportDTO();
        dto.colors = this.state.colors.map(c => {
            const r = this.toColor(c.substring(1, 3));
            const g = this.toColor(c.substring(3, 5));
            const b = this.toColor(c.substring(5, 7));
            return (r << 16) | (g << 8) | b;
        });

        await axios.request({
            method: "post",
            url: "/api/Report/Colors",
            data: dto
        });

        this.tasks.next();*/
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

    public render() {
        const {
            foregroundColor, 
            backgroundColor, 
            colors,
            face,
            emotion
        } = this.state;

        const setColor = (pos: number, color: string) => {
            const newColors = [...this.state.colors];
            newColors[pos] = color;
            this.setState({
                colors: newColors
            }) 
        }

        const getColor = (pos: number) => {
            return colors[pos];
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
        return <div>
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
                    <div>Emotion: {emotion}</div>
                </div>
            }
            <Webcam 
                ref={r => this.setRef(r)}
                audio={false}
                height={480}
                screenshotFormat="image/jpeg"
                width={640}
            />
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
