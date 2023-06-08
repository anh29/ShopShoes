import React from 'react';
import ReactDOM from 'react-dom/client';
import SelectProduct from './Components/selectProduct';
import SelectCustomer from './Components/selectCustomer';
import reportWebVitals from './reportWebVitals';
import SelectClient from './Components/selectClient';

const selectProduct = ReactDOM.createRoot(document.getElementById('selectProduct'));
selectProduct.render(
  <React.StrictMode>
    <SelectProduct />
  </React.StrictMode>
);

const customer = ReactDOM.createRoot(document.getElementById('customer'));
customer.render(
  <React.StrictMode>
    <SelectCustomer/>
  </React.StrictMode>
);

const client = ReactDOM.createRoot(document.getElementById('client'));
client.render(
  <React.StrictMode>
    <SelectClient/>
  </React.StrictMode>
);


// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
