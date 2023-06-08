import React, { Component } from 'react';
let oldQuantity, maxQuantity;
const $ = document.querySelector.bind(document);
function getOldValue(id) {
    const selectQuantityStr = '#' + id + ' .Quantity';
    oldQuantity = $(selectQuantityStr).value;    
}

function setQuantity(id) {
    const QuantityEle = $('#' + id + ' .Quantity');    
    const listIdProduct = JSON.parse(localStorage.getItem("myProducts"));
    listIdProduct.forEach((obj) => {
        if("G" + obj.id === id) {
            if(QuantityEle.value === "" || QuantityEle.value < 0) {     
                QuantityEle.value = 0;
            }    
            obj.quantity = parseInt(QuantityEle.value);
            maxQuantity = obj.max_quantity;
        } 
    });        
    localStorage.setItem("myProducts", JSON.stringify(listIdProduct));    
    console.log(JSON.parse(localStorage.getItem("myProducts")));
}

function changeCal(id, Delete) {
    const QuantityEle = $('#' + id + ' .Quantity');
    const PriceEle = $( '#' + id + ' .Price');
    const totalPrice = $('#cal .total-price');
    const total = $('#cal .total-pay');       
    const totalQuantity = $('#cal .total-quantity');    
    if(QuantityEle.value === "" || QuantityEle.value < 0) {     
        QuantityEle.value = 0;
    }    
    if (QuantityEle.value > maxQuantity) {
        QuantityEle.style.color = 'red';
    }else QuantityEle.style.color = 'black';
    const minus = parseFloat(PriceEle.value) * oldQuantity;
    let plus = 0;
    if(Delete === true) {
        QuantityEle.value = 0;
    } else plus = parseFloat(PriceEle.value) * parseInt(QuantityEle.value);
    totalPrice.value = parseFloat(totalPrice.value) - minus + plus;
    total.value = parseFloat(total.value) - minus + plus;
    totalQuantity.value = parseInt(totalQuantity.value) - oldQuantity + parseInt(QuantityEle.value);  
    oldQuantity = parseInt(QuantityEle.value); 
}

function deleteProduct(id) {
    const listIdProduct = JSON.parse(localStorage.getItem("myProducts"));
    listIdProduct.forEach((product, index) => {
        if("G" + product.id === id) {
            changeCal(id, true);
            const deleteCard = document.getElementById(id);
            deleteCard.classList.add("closeProduct");
            listIdProduct.splice(index, 1);
            localStorage.setItem('myProducts', JSON.stringify(listIdProduct));
        }
    });
    console.log(listIdProduct);
}
class printProduct extends Component {
    render() {
        return (
            <div className="box-product">             
                <img className="Picture" src={this.props.Link} alt='true'/>
                <label className="Name">{this.props.Name}</label>
                <input className="Quantity" type="number" defaultValue={this.props.Quantity} id={this.props.id} onChange={e => setQuantity(e.target.id)} onBlur={e => changeCal(e.target.id)} onFocus={e => getOldValue(e.target.id)}/>
                <input readOnly className="Price" defaultValue={this.props.Price}></input>
                <button className="Delete" id={this.props.id} onFocus={e => getOldValue(e.target.id)} ><i id={this.props.id} onClick={e => deleteProduct(e.target.id)} className="fa-sharp fa-solid fa-trash" /></button>
          </div>
        );
    }
}
export default printProduct;