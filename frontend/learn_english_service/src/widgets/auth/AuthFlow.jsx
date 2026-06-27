import { useDispatch, useSelector } from "react-redux";
import Modal from "../../modal/Modal";
import EmailSignInForm from "../signin/EmailSignInForm";
import ConfirmCodeForm from "../verify/ConfirmCodeForm";
import { closeModalAuth } from "../../redux/slices/modalSlice";
import styles from '../auth/auth_flow.module.css';
import { resetStep, resetTempUser } from "../../redux/slices/authSlice";
import { X } from "lucide-react";
import ButtonX from "../../ui/button_x/ButtonX";

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
                <ButtonX onClick={closeModal}/>
                {step === '' && <EmailSignInForm/>}
                {step === 'verify' && <ConfirmCodeForm/>}
            </div>
        </Modal>
    )
}

export default AuthFlow;