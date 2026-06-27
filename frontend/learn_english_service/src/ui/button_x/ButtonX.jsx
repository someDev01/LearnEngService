import { X } from "lucide-react";

function ButtonX({onClick}){
    return(
        <div style={{
            display:'flex',
            alignContent:'center',
            justifyContent:'end',
            width: '100%',
            padding: '4px',
            flex: '1'
        }}>
            <button style={{
                userSelect: 'none',
                cursor: 'pointer',
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                textAlign: 'center',
                backgroundColor: 'transparent', 
                outline: 'none', 
                border: '1px solid #282828', 
                borderRadius: '8px',
                width: '28px',
                height: '28px'
            }} onClick={onClick}><X size={28} color="white"/></button>
        </div>
    )
}

export default ButtonX;