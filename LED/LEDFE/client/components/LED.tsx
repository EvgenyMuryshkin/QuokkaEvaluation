import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as WeatherForecastsState from '../store/WeatherForecasts';
import * as Rx from "rxjs";
import axios from "axios";
import { debounceTime } from 'rxjs/operators';
import { ReportDTO } from '../dto/report';

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

}
class LED extends React.Component<WeatherForecastProps, IState> {
    tasks = new Rx.Subject<{}>();
    subscription: Rx.Subscription;

    constructor(props: WeatherForecastProps) {
        super(props);
        this.state = {
            foregroundColor: "#FFFFFF",
            backgroundColor: "#000000",
            colors: [...Array(64).keys()].map(pos => "#000000")
        }

        this.subscription = this.tasks.pipe(debounceTime(2000)).subscribe(_ => this.onReport());
    }

    async onReport() {
        const dto = new ReportDTO();
        dto.colors = this.state.colors.map(c => parseInt(c.substring(1), 16));

        await axios.request({
            method: "post",
            url: "/api/Report/Colors",
            data: dto
        });

        this.tasks.next();
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
        const {foregroundColor, backgroundColor, colors} = this.state;

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
        </div>;
    }

    private renderForecastsTable() {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
            {this.props.forecasts.map(forecast =>
                <tr key={ forecast.dateFormatted }>
                    <td>{ forecast.dateFormatted }</td>
                    <td>{ forecast.temperatureC }</td>
                    <td>{ forecast.temperatureF }</td>
                    <td>{ forecast.summary }</td>
                </tr>
            )}
            </tbody>
        </table>;
    }

    private renderPagination() {
        let prevStartDateIndex = (this.props.startDateIndex || 0) - 5;
        let nextStartDateIndex = (this.props.startDateIndex || 0) + 5;

        return <p className='clearfix text-center'>
            <Link className='btn btn-default pull-left' to={ `/fetchdata/${ prevStartDateIndex }` }>Previous</Link>
            <Link className='btn btn-default pull-right' to={ `/fetchdata/${ nextStartDateIndex }` }>Next</Link>
            { this.props.isLoading ? <span>Loading...</span> : [] }
        </p>;
    }
}

export default connect(
    (state: ApplicationState) => state.weatherForecasts, // Selects which state properties are merged into the component's props
    WeatherForecastsState.actionCreators                 // Selects which action creators are merged into the component's props
)(LED);
