import styles from '../edit_note_input/edit_note_input.module.css';

function EditNoteInput({text, value, onInputChange, placeholder, maxLength, isTextArea = false}){
    return(
        <div className={styles.edit_input}>
            <label>{text}</label>
            {isTextArea ? 
            <textarea 
                value={value}
                onChange={onInputChange}
                maxLength={maxLength} 
                placeholder={placeholder}
            />: 
            <input 
                value={value}
                onChange={onInputChange}
                maxLength={maxLength} 
                placeholder={placeholder}
            />}
        </div>
    )
}

export default EditNoteInput;