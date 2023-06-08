import React, { Component } from 'react';

class toastMessage extends Component {
    render() {
        return (
            <div className={this.props.name_class} style={{ backgroundColor: this.props.color, display: 'flex' }}>
                {this.props.content}
            </div>
        );
    }
}
export default toastMessage;