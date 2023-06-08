import React, { Component } from 'react';

class printProduct extends Component {
    render() {
        return (
            <div className="box-product">             
                <img className="Picture" src={this.props.Link} />
                <label className="Name">{this.props.Name}</label>
                <input className="Quantity" type="number" defaultValue={this.props.Quantity} />
                <label className="Price">{this.props.Price}</label>
                <button className="Delete"><i className="fa-sharp fa-solid fa-trash" /></button>
          </div>
        );
    }
}
export default printProduct;