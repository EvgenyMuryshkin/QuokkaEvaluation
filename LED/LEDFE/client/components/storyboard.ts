import * as Rx from "rxjs";

export interface IStoryboardItem {
    servos: number[]
}

export interface IStoryboard {
    name: string;
    Items: IStoryboardItem[]
}

export class Storyboard {
    async delay(ms: number) {
        await Rx.timer(ms).toPromise();
    }

    public async run(
        storyboard: IStoryboard,
        getter: () => number[],
        setter: (data: number[]) => void
    ) {
        // validate
        const current = getter();
        const fault = storyboard.Items.find(i => i.servos.length != current.length || !!i.servos.find(v => v < 0 || v > 180))
        if (fault) {
            alert("Servos length mismatch in storyboard");
            return;
        }

        //run
        for(const item of storyboard.Items) {
            while(true) {
                const current = [...getter()];

                if (JSON.stringify(current) === JSON.stringify(item.servos)) {
                    // done with current item
                    break;
                }

                for(let idx = 0; idx < current.length; idx++) {
                    const delta = Math.sign(item.servos[idx] - current[idx]);

                    current[idx] = current[idx] + delta;
                }
    
                setter(current);
    
                await this.delay(10);
            }
        }
    }
}