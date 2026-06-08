import ContinueButtonLoader from '../button_loader/ContinueButtonLoader';
import styles from '../button_save_note/button_save_note.module.css';

function ButtonSaveNote({onClick, loadingSave}){
    return(
        <div className={styles.wrapper}>
            <div className={styles.button_save} onClick={onClick}>
                {loadingSave ? <ContinueButtonLoader/> : <p>Сохранить</p>}
            </div>
        </div>
    )
}

export default ButtonSaveNote;