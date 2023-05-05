import React from 'react';
import ReactDOM from 'react-dom/client';
import SelectProduct from './Components/selectProduct';
import SelectCustomer from './Components/selectCustomer';
import reportWebVitals from './reportWebVitals';
import SelectClient from './Components/selectClient';
import Pay from './Components/pay';
import RootProduct from './Components/printRootProduct';
import AddCustomer from './Components/addCustomer';

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

const pay = ReactDOM.createRoot(document.getElementById('pay'));
pay.render(
  <React.StrictMode>
    <Pay/>
  </React.StrictMode>
);

const rootProduct = ReactDOM.createRoot(document.getElementById('listProduct'));
rootProduct.render(
  <React.StrictMode>
    <RootProduct/>
  </React.StrictMode>
);

const addcustomer = ReactDOM.createRoot(document.getElementById('addCustomer'));
addcustomer.render(
  <React.StrictMode>
    <AddCustomer/>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
