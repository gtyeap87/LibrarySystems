import { AppLabelProps } from "@/interfaces/AppLabel";

const AppLabel = (props: AppLabelProps) => {
  return (
    <label
      htmlFor={props.htmlFor}
      className="block text-sm font-medium text-gray-700 mb-1"
    >
      {props.text}
      {props.required && (
        <span style={{ color: "#960000", marginLeft: "4px" }}>*</span>
      )}
    </label>
  );
};

export default AppLabel;
