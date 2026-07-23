import styles from '../form_icon/form_icon.module.css';

function FormIcon({children}){
    return(
        <div className={styles.form_icon}>{children}</div>
    )
}

export default FormIcon;