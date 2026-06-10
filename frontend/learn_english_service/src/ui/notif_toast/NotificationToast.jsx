import { ToastContainer } from "react-toastify";
import '../notif_toast/notif_toast.css';

function NotificationToast(){
    return(
        <>
            <ToastContainer
                position='bottom-right'
                autoClose={2000}
                hideProgressBar={true}
                pauseOnFocusLoss={false}
                pauseOnHover={false}
                theme='dark'
            />
        </>
    )
}

export default NotificationToast;