import react from 'react';
import Axios from 'axios';
import ReactDOM from 'react-dom/client';
import ToastMessage from './toastMessage';
const addCustomer = () => {        
    const [customer, setCustomer] = react.useState([]);
    react.useEffect(()=>{
        const loadCustomer = async() => {
            const repsonse = await Axios.get("http://localhost:4000/customer");
            setCustomer(repsonse.data.result);
        }
        loadCustomer();
    }, []);

    function Add() {
        const $ = document.getElementById.bind(document);
        let nameEle = $('Name');
        // let birthEle = $('birth');
        let number_phoneEle = $('number_phone');
        let addressEle = $('address');    
        let emailEle = $('Email');    
        // const now = new Date();
        // const time_create = `${now.getMonth() + 1}/${now.getDate()}/${now.getFullYear()} ${now.getHours()}:${now.getMinutes()}:${now.getSeconds()}`;
        // let genderEle = document.getElementsByName('gender');
        // let gender;
        // for (let i = 0; i < genderEle.length; i++) {
        //     if (genderEle[i].checked) {
        //         gender = genderEle[i].value;
        //         break;
        //     }
        // }
        let warningBox = ReactDOM.createRoot($('warning'));
        if(number_phoneEle.value !== '' || nameEle.value !== '') {
            console.log(number_phoneEle.value);
            let check = customer.some((cus) => {
                return cus.phoneNumber === number_phoneEle.value;
            });
            if(check === false) {
                let object = {
                    FullName: nameEle.value,
                    PhoneNumber: number_phoneEle.value,
                    accumulatedPoints: 0,
                    Address: addressEle.value,
                    Email: emailEle.value,
                }
                console.log(object);
                fetch('http://localhost:4000/customer', {
                    method: 'POST',
                    body: JSON.stringify(object),
                    headers: {
                        "Content-Type": "application/json"
                    }
                }).then((response) => {
                    response.json().then((res) => {
                        console.log(res.result);
                        if(res.result !== '') {
                            let customerEle = document.querySelector(".input-box-customer");
                            customerEle.id = res.result;
                            customerEle.value = nameEle.value + "-" + number_phoneEle.value;
                        }
                    });
                }).catch((err) => console.log(err));
                const modal = document.querySelector(".modal");
                modal.classList.remove('open');                    
                const warningBox = ReactDOM.createRoot($('warning'));
                warningBox.render(
                    <react.StrictMode>
                        <ToastMessage content='Thêm khách hàng thành công' name_class='warningBox' color ='#03c000'></ToastMessage>
                    </react.StrictMode>
                )   
            } else {
                warningBox.render(
                    <react.StrictMode>
                        <ToastMessage content='Số điện thoại đã tồn tại !!!' name_class='warningBox'></ToastMessage>
                    </react.StrictMode>
                )
            }
        }else {
            warningBox.render(
                <react.StrictMode>
                    <ToastMessage content='Vui lòng điền tên và sô điện thoại !!!' name_class='warningBox'></ToastMessage>
                </react.StrictMode>
            )
        }
    }

    function Cancel() {
        const modal = document.querySelector(".modal");
        modal.classList.remove('open');        
        document.getElementById('Name').value = '';
        document.getElementById('birth').value = '';
        document.getElementById('number_phone').value = '';
        document.getElementById('address').value = '';    
        document.getElementById('Email').value = '';    
    }
    return (
        <div>            
            <button id="OK" onClick={() => Add()}><i className="fa-solid fa-check"></i> OK</button>
            <button id="Cancel" onClick={() => Cancel()}><i className="fa-solid fa-xmark"></i> CANCEL</button>
        </div>
    );
}

export default addCustomer;