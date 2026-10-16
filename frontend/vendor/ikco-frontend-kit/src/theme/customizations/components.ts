import type { Components, Theme } from '@mui/material';
import { dataDisplayCustomizations } from './CustomComponents/dataDisplay';
import { dataGridCustomizations } from './CustomComponents/dataGrid';
import { datePickersCustomizations } from './CustomComponents/datePicker';
import { drawerCustomization } from './CustomComponents/drawer';
import { inputsCustomizations } from './CustomComponents/inputs';
import { surfacesCustomizations } from './CustomComponents/surfaces';
import { navigationCustomizations } from './CustomComponents/navigation';
import { feedbackCustomizations } from './CustomComponents/feedback';
import { appBarCustomization } from './CustomComponents/appBar';


const components: Components<Theme> = {
    ...appBarCustomization,
    ...dataDisplayCustomizations,
    ...dataGridCustomizations,
    ...datePickersCustomizations,
    ...drawerCustomization,
    ...inputsCustomizations,
    ...surfacesCustomizations,
    ...navigationCustomizations,
    ...feedbackCustomizations,
};

export default components;

