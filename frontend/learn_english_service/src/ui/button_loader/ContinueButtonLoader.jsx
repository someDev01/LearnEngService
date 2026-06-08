import styles from '../button_loader/button_loader.module.css';

function ContinueButtonLoader(){
    return(
        <img className={styles.loader} src="https://upload.wikimedia.org/wikipedia/commons/a/ad/YouTube_loading_symbol_3_%28transparent%29.gif" alt=""/>
    )
}

export default ContinueButtonLoader;