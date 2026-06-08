import { Mail, User } from 'lucide-react';
import styles from '../input/input_field.module.css';
import ErrorText from '../error/ErrorText';

function InputField({value, setValue, error, onErrorClear}){
    return(
        <div className={`${styles.wrapper} ${styles.email}`}>
            <div className={styles.icon}>
                <Mail/>
            </div>
            <input
                className={styles.input}
                type="email"
                name="email"
                value={value}
                onChange={(e) => {
                    setValue(e.target.value);
                    onErrorClear();
                }}
                placeholder="email"
            />
            {error && <ErrorText>{error}</ErrorText>}
        </div>
    )
}

export default InputField;