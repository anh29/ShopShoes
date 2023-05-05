import React from 'react';
import ReactDOM from 'react-dom/client';
import RootProduct from './printRootProduct';
import Warning from './warningBox';
import Bill from './bill'
function addBill() {  
    let check = true;
    const billState = "Hoan thanh";
    const paymentState = "true";
    const cusElement = document.querySelector('.input-box-customer');
    const clientElement = document.querySelector('.input-box-client');
    const now = new Date();
    const time_bill = `${now.getMonth() + 1}/${now.getDate()}/${now.getFullYear()} ${now.getHours()}:${now.getMinutes()}:${now.getSeconds()}`;
    const total_quantityElement = document.querySelector('.total-quantity');
    const total_priceElement = document.querySelector('.total-price');
    const discountElement = document.querySelector('.discount');
    const extra_payElement = document.querySelector('.extra-pay');
    const total_payElement = document.querySelector('.total-pay');
    const note = document.querySelector('#note');
    const listIdProduct = JSON.parse(localStorage.getItem("myData"));
    if(listIdProduct === null) check = false;
    else listIdProduct.forEach((product) => {
        if (product.quantity > product.max_quantity) {
            check = false;
        }
    });

    if (parseInt(total_priceElement.value) !== 0) { 
        if (check === true) {    
            const object = {
                billState: billState,
                paymentState: paymentState,
                ID_User: parseInt(clientElement.id),
                ID_Customer: parseInt(cusElement.id),
                createDay: time_bill,
                productsMoney: parseFloat(total_priceElement.value),
                Discount: parseFloat(discountElement.value),
                extraMoney: parseFloat(extra_payElement.value),
                Total: parseFloat(total_payElement.value),
                Note: note.value
            }
            fetch('http://localhost:4000/bill', {
                method: 'POST',
                body: JSON.stringify(object),
                headers: {
                    "Content-Type": "application/json"
                }
            }).then(function(response) {
                response.json().then((res) => {
                    if(res.result !== '') {                    
                        listIdProduct.forEach(element => {
                            const objDetailBill = {
                                ID_bill: res.result,
                                ID_product: element.id,
                                Quantity: element.quantity
                            }
                            fetch('http://localhost:4000/detailBill', {
                                method: 'POST',
                                body: JSON.stringify(objDetailBill),
                                headers: {
                                    "Content-Type": "application/json"
                                }
                            }).then(function(response_detail) {
                                return response_detail.json();
                            });                        
                            const objUpdateProduct = {
                                SoLuongBan: element.quantity,
                                IDProduct: element.id
                            }
                            fetch('http://localhost:4000/product', {
                                method: 'PUT',
                                body: JSON.stringify(objUpdateProduct),
                                headers: {
                                    "Content-Type": "application/json"
                                }
                            }).then((response_product) => {
                                return response_product.json();
                            });
                        });        
                        if (cusElement.id !== '') {
                            const upCore = {
                                id: parseInt(cusElement.id),
                                DiemTang: 10
                            }                        
                            fetch('http://localhost:4000/customer', {
                                method: 'PUT',
                                body: JSON.stringify(upCore),
                                headers: {
                                    "Content-Type": "application/json"
                            }
                            }).then((response_cus) => {
                                return response_cus.json();
                            });
                        }

                        const selectProduct = ReactDOM.createRoot(document.getElementById('printed_bill'));
                        selectProduct.render(
                        <React.StrictMode>
                            <Bill listProduct= {JSON.parse(localStorage.getItem("myData"))} billName ={"HOÁ ĐƠN (" + res.result + ")"} billTime={time_bill} nameCus = {cusElement.value} totalProducts={total_priceElement.value} discount={discountElement.value} total={total_payElement.value}/>
                        </React.StrictMode>
                        );                                    
                        const bill = document.querySelector('.container_bill');
                        bill.classList.remove('close');   

                        const rootProduct = ReactDOM.createRoot(document.getElementById('listProduct'));
                        rootProduct.render(
                        <React.StrictMode>
                            <RootProduct/>
                        </React.StrictMode>
                        );
                        total_quantityElement.value = 0;
                        total_priceElement.value = 0;
                        discountElement.value = 0;
                        extra_payElement.value = 0;
                        total_payElement.value = 0;
                        cusElement.value = '';
                        clientElement.value = '';
                        note.value = '';
                        localStorage.setItem("myData", JSON.stringify([]));
                        const warningBox = ReactDOM.createRoot(document.getElementById('warning'));
                        warningBox.render(
                            <React.StrictMode>
                                <Warning content='Cập nhật hoá đơn thành công' name_class='warningBox' color ='#03c000'></Warning>
                            </React.StrictMode>
                        )
                    }else {
                        console.log("Tạo hoá đơn khong thành công");
                    }
                });
            });   
        }  else {
            const warningBox = ReactDOM.createRoot(document.getElementById('warning'));
            warningBox.render(
                <React.StrictMode>
                    <Warning content='Vượt quá số lượng tồn kho !!!' name_class='warningBox' ></Warning>
                </React.StrictMode>
            )
        }
    } 
}

const pay = () => {  
    return (                  
        <button className="pay" onClick={() => addBill()}>THANH TOÁN</button>
    );
} 
export default pay