import React, { Component } from 'react';
function printFunc() {
    let btn = document.querySelector('.container_bill');
    btn.classList.add('close');
}
class bill extends Component {    
    render() {
     return (
        <div className="printed_bill">
            <div className="header-bill">
                <div className="shopName">Twinkle Toes</div>
                <input type="text" className="billName" readOnly defaultValue={this.props.billName} />
            </div>
            <div className="infor-buy">
                <label>Thời gian/ When: </label>
                <input type="datetime" defaultValue={this.props.billTime} readOnly />
                <label>Khách hàng: </label>
                <input type="text" defaultValue={this.props.nameCus} readOnly />
            </div>
            <div className="infor-product">
            <table>
                <thead>
                    <tr>
                        <th>Sản phẩm/Product</th>
                        <th>SL/Count</th>
                        <th>T.Tiền/Amount</th>
                    </tr>
                </thead>
                <tbody>   
                {
                    this.props.listProduct.map((product) => 
                        <tr key={product.id}>
                            <td>{product.name}</td>
                            <td>{product.quantity}</td>
                            <td>{product.price * product.quantity}</td>
                        </tr>               
                    )
                }
                </tbody>
            </table>
            <div style={{textAlign: 'center'}}>----------------------------------------------------</div>
            <div className="Money">                
                <label>Tổng tiền hàng:</label>
                <input type="text" defaultValue={this.props.totalProducts} readOnly />
                <label>Chiếc khấu/discount:</label>
                <input type="text" defaultValue={this.props.discount} readOnly />
                <label>Tổng/Total Amount:</label>
                <input type="text" defaultValue={this.props.total} readOnly />
            </div>
            </div>         
            <div className="footer-bill">
                <div>Hàng đã mua vui lòng không đổi trả</div>
                <div>Trân trọng cảm ơn quý khách :)</div>
            </div>   
            <div className="nav">
                <button style={{backgroundColor: '#6096B4', width: '65px'}} onClick={() => printFunc()}>OK</button>
                <button style={{backgroundColor: 'rgb(248, 122, 122)'}} onClick={() => printFunc()}>Cancel</button>
            </div>
        </div>
    );         
    }

}

export default bill;