import * as React from 'react';
import "./Home.css"
import MonacoEditor from 'react-monaco-editor';
import axios, { AxiosRequestConfig, AxiosResponse } from "axios";

interface IProps {

}

interface IState {
    servos: number[];
    scriptValue: string;
}


export class Home extends React.Component<IProps, IState> {
    static displayName = Home.name;
    queue: (() => Promise<Partial<IState>>)[] = [];

    constructor(props: IProps) {
        super(props);
        this.state = {
            servos: [90, 90, 90, 90],
            scriptValue: localStorage.getItem("code") || "changeServo(0, 20)"
        }
    }

    componentDidMount() {
        //this.sendServos();
    }

    async processQueue() {
        if (!this.queue[0])
            return;

        const item = this.queue[0];
        this.queue = [...this.queue.slice(1)];

        const newState = await item();

        this.setState(newState as unknown as Pick<IState, keyof(IState)>, () => this.processQueue());
    }

    async sendServos(): Promise<boolean> {
        try {
            const { servos } = this.state;

            await axios.post("api/device/send", {
                s0: servos[0],
                s1: servos[1],
                s2: servos[2],
                s3: servos[3]
            });

            return true;
        }
        catch(e) {
            return false;
        }
    }


    async sendServosLoop() {
        while(true) {
            if (!await this.sendServos()) {
                await new Promise(resolve => setTimeout(resolve, 1000));
            }
        }
    }

    render() {
        var servos = this.state.servos;

        const setManualServo = (idx: number, value: number) => {
            var newServos = [...servos];
            newServos[idx] = value;

            this.setState({
                servos: newServos
            });
        }

        const setServo = (idx: number, value: number) => {

            this.queue.push(async () => {
                var currentServos = this.state.servos;

                var newServos = [...currentServos];
                newServos[idx] = value;

                return { servos: newServos }
            });
        }

        const delay = (ms: number) => {
            this.queue.push(async () => {
                await new Promise(resolve => setTimeout(resolve, ms));
                return {};
            })
        }

        const setServos = (values: number[]) => {
            this.queue.push(async () => {
                return { servos: values };
            })
        }

        const sendServos = () => {
            this.queue.push(async () => {
                await this.sendServos();
                return {};
            })
        }

        const measure = () => {
            this.queue.push(async () => {
                await this.sendServos();
                return {};
            })
        }

        const runScript = () => {
            try {
                eval(this.state.scriptValue);
                this.processQueue();
            }
            catch(e) {
                alert(e)
            }
        }

        return (
            <div>
                <div>
                    {servos.map((s, idx) => <div key={idx}>
                        S{idx}:
                    <input
                            className="servo-value-slider"
                            type="range"
                            min="0"
                            max="180"
                            value={s}
                            onChange={e => {
                                var newServos = [...servos];
                                newServos[idx] = parseInt(e.target.value);
                                this.setState({ servos: newServos })
                            }} />
                        <input className="servo-value" readOnly value={s} onChange={e => setManualServo(idx, parseInt(e.target.value))} />
                    </div>)}
                </div>
                <div>
                    <div className="script-run">
                        <button onClick={e => runScript()}>Run ({this.queue.length})</button>
                        <button onClick={r => { this.queue = [] }}>Stop</button>
                    </div>
                    <MonacoEditor
                        width="800"
                        height="600"
                        language="javascript"
                        theme="vs-dark"
                        value={this.state.scriptValue}
                        options={{
                            selectOnLineNumbers: true
                        }}
                        onChange={(newValue) => {
                            this.setState({ scriptValue: newValue }, () => {
                                localStorage.setItem("code", newValue)
                            });
                        }}
                    />
                </div>
            </div>
        );
  }
}
