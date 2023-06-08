import react from 'react';
import Axios from 'axios';
import ReactDOM from 'react-dom/client';
import PrintProduct from './printProduct';
window.addEventListener('beforeunload', function() {
    localStorage.clear();    
    const Obj = {check : false}; 
    localStorage.setItem("checkDiscount", JSON.stringify(Obj));
    localStorage.setItem("myProducts", JSON.stringify([]));
    localStorage.setItem("myCustomer", JSON.stringify({}));
});

let selectProduct = () => {         
    const [product, setProduct] = react.useState([]);
    const [suggestion, setSuggestion] = react.useState([]);
    const [text, setText] = react.useState('');
    const $ = document.querySelector.bind(document);
    let totalQuantity = $('#cal .total-quantity');
    let totalPrice = $('#cal .total-price');
    let discount = $('#cal .discount');
    let extra_pay = $('#cal .extra-pay');
    let total = $('#cal .total-pay');       
    react.useEffect(()=>{
        const loadProduct = async() => {
            const repsonse = await Axios.get("http://localhost:4000/product");
            setProduct(repsonse.data.result);
        }
        loadProduct();
    }, []);

    function disFunc() {
        if(discount.value >= 0) {            
            if(totalPrice.value > 0) total.value = parseFloat(totalPrice.value) - parseFloat(discount.value);
        }
        else discount.value = 0;
    }       
    discount.addEventListener("change", disFunc);

    function extraFunc() {
        if(extra_pay.value >= 0) {
            if( totalPrice.value > 0) total.value = parseFloat(total.value) + parseFloat(extra_pay.value);
        } 
        else extra_pay.value = 0;
    }       
    extra_pay.addEventListener("change", extraFunc);

    function createProduct(MaHang, TenHang, GiaHang, Link, SoLuong) { 
        let listProduct = JSON.parse(localStorage.getItem("myProducts"));
        let list = [];
        let isExit;
        if(listProduct !== null) {
            listProduct.forEach((product) => {
                list.push(product);
            }) 
        }
        if (list === '') isExit = false;
        else isExit = list.some(function(product) {
            return product.id === MaHang;
        });
        if(isExit === true) {
            const quantity = $("#G" + MaHang +  " .Quantity");
            quantity.value = parseFloat(quantity.value) + 1;
            listProduct.forEach((obj) => {
                if(obj.id === MaHang) {
                    obj.quantity++;
                } 
            });    
            localStorage.setItem("myProducts", JSON.stringify(list));    
        }
        else{
            let printProduct = ReactDOM.createRoot(document.getElementById("G" + MaHang));  
            printProduct.render(
                <react.StrictMode>        
                    <PrintProduct Name={TenHang} Price={GiaHang} Quantity={1} Link={Link} id={("G" + MaHang)}/>     
                </react.StrictMode>
            );     
            let newObject = {id: MaHang, name: TenHang, quantity: 1, price: GiaHang, max_quantity: SoLuong};
            //chua biet add truc tiep vao localstorage
            list.push(newObject);
            localStorage.setItem("myProducts", JSON.stringify(list));
        }           
        let productCard = $("#G" + MaHang);
        productCard.classList.remove("closeProduct");     
    }    
    const choseProduct = (product) => {
        let selectQuantity = 1; 
        setSuggestion([]); 
        setText('');        
        createProduct(product.Id, product.Title, product.Price, product.Image, product.Quantity);
        totalQuantity.value = parseInt(totalQuantity.value) + parseInt(selectQuantity);
        totalPrice.value = parseInt(totalPrice.value) + product.Price * parseFloat(selectQuantity);
        const Dis = JSON.parse(localStorage.getItem("checkDiscount"));
        if(Dis.check === true) {
            discount.value = parseFloat(totalPrice.value) * 0.2;
        }
        total.value =  parseInt(totalPrice.value) + parseFloat(extra_pay.value) - parseFloat(discount.value);
    }

    const onChangeHandler = (text) => {
        let result = [];
        if(text.length > 0) {
            result = product.filter((keyword) => {
                return keyword.Title.toLowerCase().includes(text.toLowerCase());
            });
        }
        setSuggestion(result);
        setText(text);
    }

    return (                  
        <div className="find-product">                    
            <input type="text" id="input-box" className="input" style={{paddingLeft: '30px'}} placeholder="Tìm sản phẩm" autoComplete="off" onChange={e =>onChangeHandler(e.target.value)}
        value={text}/>
            <i className="icon-Find1 fa-solid fa-magnifying-glass" />
            <div className="result-box">       
                {suggestion.map((product) =>
                    <li key={product.Id} className='extra-product' onClick = {() => choseProduct(product)}>{product.Title}</li>
                )}
            </div>
        </div>
    );
} 
export default selectProduct

// const App = () => {
//   const [data, setData] = useState();
//   const getData = async function () {
//     const response = await Axios.get("http://localhost:4000/product/id");
//     setData(response.data);
//   }
//   useEffect(()=> {
//     getData();
//   },[])
//   return (
//     <div>
//       <h1>Product list</h1>
//       <ul>
//         {data.map((product, index) => (
//           <li key={index}>{product.TenHang}</li>
//         ))}
//       </ul>
//     </div>
//   );
// }
// export default App

// const App = () => {
//   const [data, setData] = useState([]);

//   useEffect(() => {
//     Axios.get('http://localhost:4000/product')
//       .then(response => setData(response.data))
//       .catch(error => console.error(error));
//   }, []);
  
//   let availableKeywords = [];
//   data.map((product, index) => (
//     availableKeywords.push(product.TenHang)
//   ));

//   const resultsBox = document.querySelector(".result-box");
//   const inputBox = document.getElementById("input-box");

//   inputBox.onkeyup = function() {
//       let result = [];
//       let input = inputBox.value;
//       if(input.length) {
//           result = availableKeywords.filter((keyword) => {
//               return keyword.toLowerCase().includes(input.toLowerCase());
//           });
//           console.log(result);
//       }
//       display(result);

//       if(!result.length) {
//           resultsBox.innerHTML = ' ';
//       }
//   }

//   function display(result) {
//       const content = result.map((list)=> {
//           return "<li onclick=selectInput(this)>"+ list +"</li>";
//       });

//       resultsBox.innerHTML = "<ul>" + content.join(' ') +"</ul>";
//   }

//   function selectInput(list) {
//       inputBox.value = list.innerHTML;
//       resultsBox.innerHTML = '';
//   }
// }
// export default App
