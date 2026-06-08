import useScrollLock from '../hooks/scroll_lock/useScrollLock';
import styles from '../modal/modal.module.css';

function Modal({children, isOpen}){

    useScrollLock(isOpen);
    
    if(!isOpen) return;
    
    return(
        <div className={styles.overlay}>
            {children}
        </div>
    )
}

export default Modal;