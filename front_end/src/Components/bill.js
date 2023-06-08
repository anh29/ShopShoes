import React, { Component} from 'react';
import ReactToPrint from 'react-to-print';
class bill extends Component {    
    close = () => {
        let btn = document.querySelector('.container_bill');
        btn.classList.add('close');
    }
    render() {
        return (        
            <div className="printed_bill">
                <div className='bill' ref={el=>{this.componentRef=el}}>
                    <div className="header-bill">
                        <div className="shopName">Twinkle Toes</div>
                        <input type="text" className="billName" readOnly defaultValue={this.props.billName} />
                    </div>
                    <div className="infor-buy">
                        <label>Thời gian/ When: </label>
                        <input type="datetime" defaultValue={this.props.billTime} readOnly />
                        <label style={{ display: "block" }} >Khách hàng: </label>
                        <input type="text" defaultValue={this.props.nameCus} readOnly />
                    </div>
                    <div className="infor-product">
                    <table>
                        <thead>
                            <tr>
                                <th style={{ textAlign: "left" }}>Sản phẩm/Product</th>
                                <th>SL/Count</th>
                                <th>T.Tiền/Amount</th>
                            </tr>
                        </thead>
                        <tbody>   
                        {
                            this.props.listProduct.map((product) => 
                                <tr key={product.id}>
                                    <td style={{ textAlign: "left" }}>{product.name}</td>
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
                    <label className='Note'>Ghi Chú: {this.props.note}</label>
                    </div>         
                    <div className="footer-bill">
                        <div>Hàng đã mua vui lòng không đổi trả</div>
                        <div>Trân trọng cảm ơn quý khách :)</div>
                    </div>   
                </div> 
                <div className='nav'>    
                    {/* <button id='navPrint' style={{backgroundColor: '#6096B4', width: '65px'}} onClick={() => close()}>OK</button> */}
                    <ReactToPrint
                        trigger={() => 
                            // document.querySelector("#navPrint")                            
                            <button id='navPrint' style={{backgroundColor: '#6096B4', width: '65px'}}>PRINT</button>
                        }
                        content={()=> this.componentRef}
                        documentTitle='BILL'
                        pageStyle="print"
                        onAfterPrint={() => this.close()}
                    />
                    <button style={{backgroundColor: 'rgb(248, 122, 122)'}} onClick={() => this.close()}>Cancel</button>
                </div>
            </div>
        );         
    }
}

export default bill;