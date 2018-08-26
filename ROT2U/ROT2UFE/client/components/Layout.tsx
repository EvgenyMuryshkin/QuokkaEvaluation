import * as React from 'react';

export class Layout extends React.Component<{}, {}> {
    public render() {
        return <div>
            { this.props.children }
        </div>;
    }
}