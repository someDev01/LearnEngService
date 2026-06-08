import styles from '../button_add/button_add.module.css';
import ContinueButtonLoader from '../button_loader/ContinueButtonLoader';

function ButtonAddNote({selectedText, onAddWordWithContext, noteCreating}){
    return(
        <div 
            className={styles.button_add}
            onClick={() => onAddWordWithContext(selectedText)}
        >
            <p>{noteCreating ? <ContinueButtonLoader/> : "добавить"}</p>
        </div>
    )
}

export default ButtonAddNote;