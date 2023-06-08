import React, { Component } from 'react';

class cal extends Component {
    render() {
        return (
            <div className="cal">
                <div style={{marginTop: '40px'}}>
                    <label htmlFor="title">Tổng số lượng hàng</label>
                    <input type="number" className='totalQuantity' id="title" readOnly defaultValue={this.props.totalQuantity} />
                </div>
                <div>
                    <label>Tổng tiền hàng</label>
                    <input type="number" id="title" readOnly defaultValue={this.props.totalPrice} />
                </div>
                <div>
                    <label className="TieuDe">Giảm giá</label>
                    <input type="number" className="discount" defaultValue={this.props.totalDiscount} />
                </div>
                <div>
                    <label className="TieuDe">Thu khác</label>
                    <input type="number" className="extra-pay" defaultValue={this.props.total_extra_pay} />
                </div>
                <div>
                    <label className="TieuDe">Khách cần trả</label>
                    <input type="number" id="title" readOnly defaultValue={this.props.total} />
                </div>
          </div>
        );
    }
}


export default cal;