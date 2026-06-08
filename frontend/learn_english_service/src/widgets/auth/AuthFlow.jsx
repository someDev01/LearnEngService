import { useDispatch, useSelector } from "react-redux";
import Modal from "../../modal/Modal";
import EmailSignInForm from "../signin/EmailSignInForm";
import ConfirmCodeForm from "../verify/ConfirmCodeForm";
import { closeModalAuth } from "../../redux/slices/modalSlice";
import styles from '../auth/auth_flow.module.css';
import { resetStep, resetTempUser } from "../../redux/slices/authSlice";

function AuthFlow({isOpen}){

    const step = useSelector(state => state.auth.step);
    const dispatch = useDispatch();
    
    const closeModal = () => {
        dispatch(closeModalAuth());
        dispatch(resetStep());
        dispatch(resetTempUser());
    }
    
    return(
        <Modal isOpen={isOpen}>
            <div className={styles.form}>
                <button style={{
                    userSelect: 'none',
                    cursor: 'pointer',
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    textAlign: 'center',
                    position:'absolute', 
                    top: 5, 
                    right: 5, 
                    backgroundColor: 'transparent', 
                    outline: 'none', 
                    border: '1px solid #4a4a4a', 
                    borderRadius: '6px',
                    color: 'white', 
                    fontSize: '20px',
                    width: '24px',
                    height: '24px'
                }} onClick={() => closeModal()}>x</button>
                {step === '' && <EmailSignInForm/>}
                {step === 'verify' && <ConfirmCodeForm/>}
            </div>
        </Modal>
    )
}

export default AuthFlow;