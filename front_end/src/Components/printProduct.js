import React, { Component } from 'react';
let oldQuantity, maxQuantity;

function getOldValue(id) {
    let selectQuantityStr = '#' + id + ' .Quantity';
    oldQuantity = document.querySelector(selectQuantityStr).value;    
}

function setQuantity(id) {
    let QuantityEle = document.querySelector('#' + id + ' .Quantity');    
    let listIdProduct = JSON.parse(localStorage.getItem("myData"));
    listIdProduct.forEach((obj) => {
        if("G" + obj.id === id) {
            if(QuantityEle.value === "" || QuantityEle.value < 0) {     
                QuantityEle.value = 0;
            }    
            obj.quantity = parseInt(QuantityEle.value);
            maxQuantity = obj.max_quantity;
        } 
    });        
    localStorage.setItem("myData", JSON.stringify(listIdProduct));    
    console.log(JSON.parse(localStorage.getItem("myData")));
}

function changeCal(id, Delete) {
    let QuantityEle = document.querySelector('#' + id + ' .Quantity');
    let PriceEle = document.querySelector( '#' + id + ' .Price');
    let totalPrice = document.querySelector('#cal .total-price');
    let total = document.querySelector('#cal .total-pay');       
    let totalQuantity = document.querySelector('#cal .total-quantity');    
    if(QuantityEle.value === "" || QuantityEle.value < 0) {     
        QuantityEle.value = 0;
    }    
    if (QuantityEle.value > maxQuantity) {
        QuantityEle.style.color = 'red';
    }else QuantityEle.style.color = 'black';
    let minus = parseFloat(PriceEle.value) * oldQuantity;
    let plus;
    if(Delete === true) {
        plus = 0;
        QuantityEle.value = 0;
    } else plus = parseFloat(PriceEle.value) * parseInt(QuantityEle.value);
    totalPrice.value = parseFloat(totalPrice.value) - minus + plus;
    total.value = parseFloat(total.value) - minus + plus;
    totalQuantity.value = parseInt(totalQuantity.value) - oldQuantity + parseInt(QuantityEle.value);  
    oldQuantity = parseInt(QuantityEle.value); 
}

function setValue(id) {
    changeCal(id);
}

function deleteProduct(id) {
    let listIdProduct = JSON.parse(localStorage.getItem("myData"));
    listIdProduct.forEach((product, index) => {
        if("G" + product.id === id) {
            changeCal(id, true);
            let deleteCard = document.getElementById(id);
            deleteCard.classList.add("closeProduct");
            listIdProduct.splice(index, 1);
            localStorage.setItem('myData', JSON.stringify(listIdProduct));
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
                <input className="Quantity" type="number" defaultValue={this.props.Quantity} id={this.props.id} onChange={e => setQuantity(e.target.id)} onBlur={e => setValue(e.target.id)} onFocus={e => getOldValue(e.target.id)}/>
                <input readOnly className="Price" defaultValue={this.props.Price}></input>
                <button className="Delete" id={this.props.id} onFocus={e => getOldValue(e.target.id)} ><i id={this.props.id} onClick={e => deleteProduct(e.target.id)} className="fa-sharp fa-solid fa-trash" /></button>
          </div>
        );
    }
}
export default printProduct;